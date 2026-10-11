using System.Collections.Generic;
using RimWorld;
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
            return pawn.jobs.TryTakeOrderedJob(
                JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("Stickyfoam_RemoveFoam"), pawn), JobTag.Misc);
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

            return new FloatMenuOption("Remove foam", () => FoamRemovalPolicy.OrderRemoval(clickedPawn));
        }
    }

    public class JobDriver_RemoveFoam : JobDriver
    {
        private int duration = 600;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return FoamRemovalPolicy.CanRemove(pawn);
        }

        public override void Notify_Starting()
        {
            base.Notify_Starting();
            duration = StickyfoamMod.Settings.removalDurationTicks;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref duration, "foamRemovalDuration", 600);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            AddEndCondition(() => FoamRemovalPolicy.GetCoating(pawn) == null
                ? JobCondition.Succeeded
                : FoamRemovalPolicy.CanRemove(pawn) ? JobCondition.Ongoing : JobCondition.Incompletable);
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
