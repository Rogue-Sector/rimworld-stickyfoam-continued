using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Stickyfoam
{
    [StaticConstructorOnStartup]
    public static class Stickyfoam_Tracker {
        private static Dictionary<int, PawnStickyfoamDrawer> StickyfoamDrawers = new Dictionary<int, PawnStickyfoamDrawer>();

        public static PawnStickyfoamDrawer GetDrawer(Pawn pawn)
        {
            if (!StickyfoamDrawers.ContainsKey(pawn.thingIDNumber))
            {
                var drawer = new PawnStickyfoamDrawer(pawn) {
                    coveredInFoam = pawn.health.hediffSet.HasHediff(Stickyfoam_HediffDefOf.CoveredInStickyfoam)
                };
                // Render lookups can outlive a coating; retain only live, coated pawns.
                if (!drawer.coveredInFoam || pawn.Destroyed || pawn.Discarded) return drawer;
                StickyfoamDrawers[pawn.thingIDNumber] = drawer;
            }

            return StickyfoamDrawers[pawn.thingIDNumber];
        }

        public static void RemoveDrawer(Pawn pawn)
        {
            if (pawn.thingIDNumber != -1) {
                StickyfoamDrawers.Remove(pawn.thingIDNumber);
            }
        }

        public static void ClearTracker()
        {
            StickyfoamDrawers.Clear();
        }
    }
}
