using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace Stickyfoam
{
    public class StickyfoamSettings : ModSettings
    {
        public bool damageReductionEnabled;
        public float damageReduction = 0.25f;
        public float groundBeauty = -20f;
        public float groundCleanliness = -15f;
        public float groundCleaningWork = 150f;
        public float groundLifetimeDays = 4f;
        public float movementFactor = 0.6f;
        public int coatingDurationTicks = 6000;
        public int removalDurationTicks = 600;

        public void ResetToDefaults()
        {
            damageReductionEnabled = false;
            damageReduction = 0.25f;
            groundBeauty = -20f;
            groundCleanliness = -15f;
            groundCleaningWork = 150f;
            groundLifetimeDays = 4f;
            movementFactor = 0.6f;
            coatingDurationTicks = 6000;
            removalDurationTicks = 600;
        }

        public void OriginalBalance()
        {
            ResetToDefaults();
            damageReductionEnabled = true;
        }

        private static float Bounded(float value, float min, float max, float fallback)
        {
            return float.IsNaN(value) || float.IsInfinity(value) ? fallback : Mathf.Clamp(value, min, max);
        }

        public void Validate()
        {
            damageReduction = Bounded(damageReduction, 0f, 0.75f, 0.25f);
            groundBeauty = Bounded(groundBeauty, -100f, 0f, -20f);
            groundCleanliness = Bounded(groundCleanliness, -50f, 0f, -15f);
            groundCleaningWork = Bounded(groundCleaningWork, 1f, 1500f, 150f);
            groundLifetimeDays = Bounded(groundLifetimeDays, 0.1f, 30f, 4f);
            movementFactor = Bounded(movementFactor, 0.1f, 1f, 0.6f);
            coatingDurationTicks = Mathf.Clamp(coatingDurationTicks, 60, 60000);
            removalDurationTicks = Mathf.Clamp(removalDurationTicks, 60, 3600);
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref damageReductionEnabled, "damageReductionEnabled", false);
            Scribe_Values.Look(ref damageReduction, "damageReduction", 0.25f);
            Scribe_Values.Look(ref groundBeauty, "groundBeauty", -20f);
            Scribe_Values.Look(ref groundCleanliness, "groundCleanliness", -15f);
            Scribe_Values.Look(ref groundCleaningWork, "groundCleaningWork", 150f);
            Scribe_Values.Look(ref groundLifetimeDays, "groundLifetimeDays", 4f);
            Scribe_Values.Look(ref movementFactor, "movementFactor", 0.6f);
            Scribe_Values.Look(ref coatingDurationTicks, "coatingDurationTicks", 6000);
            Scribe_Values.Look(ref removalDurationTicks, "removalDurationTicks", 600);
            Validate();
        }
    }

    public class StickyfoamMod : Mod
    {
        public static StickyfoamSettings Settings;
        private Vector2 scrollPosition;

        public StickyfoamMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<StickyfoamSettings>();
            LongEventHandler.ExecuteWhenFinished(ApplySettings);
        }

        public override string SettingsCategory() { return "Stickyfoam Continued"; }

        public static void ApplySettings()
        {
            Settings.Validate();
            HediffDef coating = DefDatabase<HediffDef>.GetNamed("CoveredInStickyfoam");
            SetStat(coating.stages[0].statFactors, StatDefOf.IncomingDamageFactor,
                Settings.damageReductionEnabled ? 1f - Settings.damageReduction : 1f);
            SetStat(coating.stages[0].statFactors, StatDefOf.MoveSpeed, Settings.movementFactor);
            coating.comps.OfType<HediffCompProperties_Disappears>().First().disappearsAfterTicks =
                new IntRange(Settings.coatingDurationTicks, Settings.coatingDurationTicks);
            ThingDef filth = DefDatabase<ThingDef>.GetNamed("Filth_Stickyfoam");
            SetStat(filth.statBases, StatDefOf.Beauty, Settings.groundBeauty);
            SetStat(filth.statBases, StatDefOf.Cleanliness, Settings.groundCleanliness);
            filth.filth.cleaningWorkToReduceThickness = Settings.groundCleaningWork;
            filth.filth.disappearsInDays = new FloatRange(Settings.groundLifetimeDays * 0.75f, Settings.groundLifetimeDays * 1.25f);
            // Settings change only on application, never in a pawn or map tick loop.
            if (Current.Game != null)
                foreach (Map map in Find.Maps)
                    foreach (Thing foam in map.listerThings.ThingsOfDef(filth))
                        foam.GetRoom()?.Notify_ContainedThingSpawnedOrDespawned(foam);
        }

        private static void SetStat(List<StatModifier> values, StatDef stat, float value)
        {
            StatModifier modifier = values.Find(item => item.stat == stat);
            if (modifier == null) values.Add(new StatModifier { stat = stat, value = value });
            else modifier.value = value;
            stat.Worker.TryClearCache();
        }

        public override void WriteSettings()
        {
            ApplySettings();
            base.WriteSettings();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.Validate();
            Rect view = new Rect(0f, 0f, inRect.width - 20f, 660f);
            Widgets.BeginScrollView(inRect, ref scrollPosition, view);
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(view);
            listing.Label("Global settings for all colonies. Changes apply when this window closes.");
            listing.CheckboxLabeled("Damage reduction", ref Settings.damageReductionEnabled);
            Settings.damageReduction = listing.SliderLabeled("Protection strength: " + Settings.damageReduction.ToString("P0"), Settings.damageReduction, 0f, 0.75f);
            listing.Label("Off by default: coated targets are easier to kill than with the original 25% damage reduction. This does not add armour.");
            listing.GapLine();
            listing.Label("Ground foam");
            Settings.groundBeauty = listing.SliderLabeled("Beauty: " + Settings.groundBeauty.ToString("F0"), Settings.groundBeauty, -100f, 0f);
            Settings.groundCleanliness = listing.SliderLabeled("Cleanliness: " + Settings.groundCleanliness.ToString("F0"), Settings.groundCleanliness, -50f, 0f);
            Settings.groundCleaningWork = listing.SliderLabeled("Cleaning work: " + Settings.groundCleaningWork.ToString("F0"), Settings.groundCleaningWork, 1f, 1500f);
            Settings.groundLifetimeDays = listing.SliderLabeled("Average lifetime (days): " + Settings.groundLifetimeDays.ToString("F1"), Settings.groundLifetimeDays, 0.1f, 30f);
            listing.GapLine();
            listing.Label("Pawn coating");
            float slowdown = 1f - Settings.movementFactor;
            slowdown = listing.SliderLabeled("Movement slowdown: " + slowdown.ToString("P0"), slowdown, 0f, 0.9f);
            Settings.movementFactor = 1f - slowdown;
            Settings.coatingDurationTicks = Mathf.RoundToInt(60f * listing.SliderLabeled("Coating duration (seconds): " + (Settings.coatingDurationTicks / 60f).ToString("F0"), Settings.coatingDurationTicks / 60f, 1f, 1000f));
            Settings.removalDurationTicks = Mathf.RoundToInt(60f * listing.SliderLabeled("Pawn removal time (seconds): " + (Settings.removalDurationTicks / 60f).ToString("F0"), Settings.removalDurationTicks / 60f, 1f, 60f));
            listing.Label("Speed, protection, ground beauty/cleanliness and cleaning work change on existing foam. Lifetimes apply to fresh coatings/hits and newly spawned ground foam; existing timers keep their remaining time. A removal job keeps the duration it started with. Seconds are game time.");
            if (listing.ButtonText("Reset to defaults")) Settings.ResetToDefaults();
            if (listing.ButtonText("Original balance")) Settings.OriginalBalance();
            listing.End();
            Widgets.EndScrollView();
        }
    }
}
