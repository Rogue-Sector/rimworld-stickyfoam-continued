using RimWorld;
using UnityEngine;
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

        public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
        {
            Vector3 offset = base.OffsetFor(node, parms, out pivot);
            if (parms.facing != Rot4.North || !parms.pawn.RaceProps.Humanlike
                || parms.pawn.apparel == null || !parms.flags.FlagSet(PawnRenderFlags.Clothes)) return offset;

            foreach (Apparel apparel in parms.pawn.apparel.WornApparel)
            {
                if (apparel.def.apparel.LastLayer == ApparelLayerDefOf.Shell && !apparel.def.apparel.shellRenderedBehindHead
                    && apparel.def.apparel.drawData == null && !apparel.def.apparel.HasDefinedGraphicProperties)
                {
                    // Preserve the normal overlay clearance when shell apparel moves from layer 20 to 88.
                    return offset + Vector3.up * (PawnRenderUtility.AltitudeForLayer(88f) - PawnRenderUtility.AltitudeForLayer(20f));
                }
            }
            return offset;
        }
    }
}
