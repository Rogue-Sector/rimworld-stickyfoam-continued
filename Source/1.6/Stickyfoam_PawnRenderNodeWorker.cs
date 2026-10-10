using RimWorld;
using Verse;

namespace Stickyfoam
{
    public class PawnRenderNodeWorker_OverlayStickyfoam : PawnRenderNodeWorker_Overlay
    {
        protected override PawnOverlayDrawer OverlayDrawer(Pawn pawn)
        {
            return Stickyfoam_Tracker.GetDrawer(pawn);
        }

        public override bool ShouldListOnGraph(PawnRenderNode node, PawnDrawParms parms)
        {
            return Stickyfoam_Tracker.GetDrawer(parms.pawn).coveredInFoam;
        }

        public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
        {
            if (base.CanDrawNow(node, parms) && parms.rotDrawMode == RotDrawMode.Fresh)
            {
                return Stickyfoam_Tracker.GetDrawer(parms.pawn).coveredInFoam;
            }
            return false;
        }
    }
}
