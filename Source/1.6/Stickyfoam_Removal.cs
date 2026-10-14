using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Stickyfoam
{
    public static class FoamRemovalPolicy
    {
        public static Hediff_CoveredInStickyfoam GetCoating(Pawn pawn)
        {
            return pawn?.health?.hediffSet.GetFirstHediffOfDef(
                Stickyfoam_HediffDefOf.CoveredInStickyfoam) as Hediff_CoveredInStickyfoam;
        }

        public static bool CanRemove(Pawn pawn)
        {
            return pawn != null && pawn.Spawned && !pawn.Dead && !pawn.Destroyed
                && pawn.RaceProps.Humanlike && pawn.IsColonistPlayerControlled
                && !pawn.Drafted && !pawn.Downed && !pawn.InMentalState
                && pawn.Awake() && !pawn.IsBurning()
                && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)
                && GetCoating(pawn) != null;
        }

        public static bool OrderRemoval(Pawn pawn)
        {
            if (!CanRemove(pawn))
                return false;

            Hediff_CoveredInStickyfoam coating = GetCoating(pawn);
            coating.autoRemovalSuppressed = false;
            coating.nextAutoRemovalTick = 0;
            // Vanilla coalesces identical ordered jobs without copying their playerForced flag.
            if (pawn.CurJob?.def.defName == "Stickyfoam_RemoveFoam")
            {
                pawn.CurJob.playerForced = true;
                return true;
            }
            return pawn.jobs.TryTakeOrderedJob(
                JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("Stickyfoam_RemoveFoam"), pawn), JobTag.Misc);
        }

        public static bool SafeForAuto(Pawn pawn)
        {
            return pawn != null && pawn.Spawned && !pawn.IsFormingCaravan()
                && pawn.mindState.duty == null && !pawn.stances.curStance.StanceBusy
                && pawn.Position.GetDangerFor(pawn, pawn.Map) == Danger.None
                && !GenHostility.AnyHostileActiveThreatToPlayer(pawn.Map, false, true);
        }

        public static bool CanAutoRemove(Pawn pawn)
        {
            if (!CanRemove(pawn) || !SafeForAuto(pawn))
                return false;

            Hediff_CoveredInStickyfoam coating = GetCoating(pawn);
            if (coating.autoRemovalSuppressed || Find.TickManager.TicksGame < coating.nextAutoRemovalTick
                || pawn.jobs.jobQueue.Count != 0)
                return false;

            Job current = pawn.CurJob;
            return current == null || (!current.playerForced &&
                (current.def == JobDefOf.Wait || current.def == JobDefOf.Wait_Wander || current.def == JobDefOf.GotoWander));
        }
    }

    public class JobGiver_RemoveFoam : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            // Preserve a running maintenance job if a normal job-override check reaches this node.
            if (pawn.CurJob?.def.defName == "Stickyfoam_RemoveFoam" && !pawn.CurJob.playerForced
                && FoamRemovalPolicy.CanRemove(pawn) && FoamRemovalPolicy.SafeForAuto(pawn))
                return pawn.CurJob;

            return FoamRemovalPolicy.CanAutoRemove(pawn)
                ? JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("Stickyfoam_RemoveFoam"), pawn)
                : null;
        }
    }

    public class FloatMenuOptionProvider_RemoveFoam : FloatMenuOptionProvider
    {
        protected override bool Drafted => false;
        protected override bool Undrafted => true;
        protected override bool Multiselect => false;
        protected override bool RequiresManipulation => true;
        protected override bool CanSelfTarget => true;

        protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
        {
            if (clickedPawn != context.FirstSelectedPawn || !FoamRemovalPolicy.CanRemove(clickedPawn))
                return null;

            return new FloatMenuOption("Stickyfoam_RemoveFoam".Translate(), () => FoamRemovalPolicy.OrderRemoval(clickedPawn));
        }
    }

    public class JobDriver_RemoveFoam : JobDriver
    {
        private int duration = 600;
        private int nextSafetyCheckTick;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return FoamRemovalPolicy.CanRemove(pawn);
        }

        public override void Notify_Starting()
        {
            base.Notify_Starting();
            // Tagged subtrees issue through Humanlike, but their giver keys belong to their own tree.
            if (job.jobGiver is JobGiver_RemoveFoam)
                job.jobGiverThinkTree = DefDatabase<ThinkTreeDef>.GetNamed("Stickyfoam_AutomaticRemoval");
            duration = StickyfoamMod.Settings.removalDurationTicks;
            nextSafetyCheckTick = 0;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref duration, "foamRemovalDuration", 600);
            duration = Mathf.Clamp(duration, 60, 3600);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            AddEndCondition(() =>
            {
                if (FoamRemovalPolicy.GetCoating(pawn) == null)
                    return JobCondition.Succeeded;
                if (!FoamRemovalPolicy.CanRemove(pawn))
                    return JobCondition.Incompletable;
                if (!job.playerForced && Find.TickManager.TicksGame >= nextSafetyCheckTick)
                {
                    nextSafetyCheckTick = Find.TickManager.TicksGame + 60;
                    if (!FoamRemovalPolicy.SafeForAuto(pawn))
                        return JobCondition.InterruptForced;
                }
                return JobCondition.Ongoing;
            });
            AddFinishAction(condition =>
            {
                Hediff_CoveredInStickyfoam coating = FoamRemovalPolicy.GetCoating(pawn);
                if (condition == JobCondition.Succeeded || coating == null)
                    return;

                if (job.playerInterruptedForced)
                    coating.autoRemovalSuppressed = true;
                else
                    coating.nextAutoRemovalTick = Find.TickManager.TicksGame + 600;
            });

            yield return Toils_General.Wait(duration).WithProgressBarToilDelay(TargetIndex.A);
            yield return Toils_General.Do(() =>
            {
                List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
                for (int i = hediffs.Count - 1; i >= 0; i--)
                {
                    if (hediffs[i].def == Stickyfoam_HediffDefOf.CoveredInStickyfoam)
                        pawn.health.RemoveHediff(hediffs[i]);
                }
            });
        }

        public override void Notify_DamageTaken(DamageInfo dinfo)
        {
            base.Notify_DamageTaken(dinfo);
            if (dinfo.Amount > 0f && dinfo.Def.harmsHealth && dinfo.Def.canInterruptJobs
                && dinfo.Def.ExternalViolenceFor(pawn))
                EndJobWith(JobCondition.InterruptForced);
        }
    }
}
