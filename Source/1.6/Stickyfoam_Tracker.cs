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
                StickyfoamDrawers[pawn.thingIDNumber] = new PawnStickyfoamDrawer(pawn);

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
