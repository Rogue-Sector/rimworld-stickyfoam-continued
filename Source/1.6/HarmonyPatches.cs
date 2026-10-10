using Stickyfoam;
using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace Stickyfoam
{
    [StaticConstructorOnStartup]
    internal class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("katana.stickyfoam");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Log.Message("[katana.stickyfoam] Loaded");
        }

        [HarmonyPatch(typeof(Verse.Profile.MemoryUtility), "ClearAllMapsAndWorld")]
        static class Patch_Clear_Stickyfoam_Tracker
        {
            [HarmonyPrefix]
            static void Prefix()
            {
                Stickyfoam_Tracker.ClearTracker();
            }
        }

        [HarmonyPatch(typeof(Pawn), nameof(Pawn.Destroy))]
        static class Patch_Destroy_Stickyfoam_Drawer
        {
            [HarmonyPostfix]
            static void Postfix(Pawn __instance)
            {
                if (__instance.Destroyed) Stickyfoam_Tracker.RemoveDrawer(__instance);
            }
        }

    }
}
