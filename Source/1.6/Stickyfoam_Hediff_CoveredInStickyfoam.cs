using RimWorld;
using Verse;

namespace Stickyfoam
{
    public class Hediff_CoveredInStickyfoam : HediffWithComps
    {
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);

            Stickyfoam_Tracker.GetDrawer(pawn).coveredInFoam = true;
        }

        public override void PostRemoved()
        {
            base.PostRemoved();

            Stickyfoam_Tracker.GetDrawer(pawn).coveredInFoam = pawn.health.hediffSet.HasHediff(Stickyfoam_HediffDefOf.CoveredInStickyfoam);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                Stickyfoam_Tracker.GetDrawer(pawn).coveredInFoam = true;
            }
        }
    }
}
