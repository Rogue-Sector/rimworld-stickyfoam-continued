using RimWorld;
using Verse;

namespace Stickyfoam
{
    public class Hediff_CoveredInStickyfoam : HediffWithComps
    {
        public bool autoRemovalSuppressed;
        public int nextAutoRemovalTick;

        public override bool TryMergeWith(Hediff other)
        {
            if (!base.TryMergeWith(other)) return false;
            // A fresh hit starts a new maintenance opportunity after an explicit cancellation.
            autoRemovalSuppressed = false;
            nextAutoRemovalTick = 0;
            return true;
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);

            Stickyfoam_Tracker.GetDrawer(pawn).coveredInFoam = true;
        }

        public override void PostRemoved()
        {
            base.PostRemoved();

            if (pawn.health.hediffSet.HasHediff(Stickyfoam_HediffDefOf.CoveredInStickyfoam))
                Stickyfoam_Tracker.GetDrawer(pawn).coveredInFoam = true;
            else
                Stickyfoam_Tracker.RemoveDrawer(pawn);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref autoRemovalSuppressed, "autoRemovalSuppressed", false);
            Scribe_Values.Look(ref nextAutoRemovalTick, "nextAutoRemovalTick", 0);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                Stickyfoam_Tracker.GetDrawer(pawn).coveredInFoam = true;
            }
        }
    }
}
