﻿using UnityEngine;

using HarmonyLib;
using RimWorld;
using Verse;

namespace CM_Less_Shitty_Ambush
{
    public class AmbushModSettings : ModSettings
    {
        private float maxEnemyFactionMultiplier = 100.0f;
        private float maxManhunterPackMultiplier = 100.0f;
        private float maxTempMapMultiplier = 100.0f;

        public float enemyFactionMultiplier = 2.0f;
        public float manhunterPackMultiplier = 2.0f;
        public float caravanVisibilityMultiplier = 1.0f;

        private int maxSecondsUntilExitMapPossible = 1000;
        public bool usePlayerMainColonyThreat = false;
        public float enemyFactionPlayerMainColonyThreatMultiplier = 0.1f;
        public float manhunterPlayerMainColonyThreatMultiplier = 0.1f;

        public float tempMapMultiplier = 2.0f;
        public float tempMapAdditionalAvgRaidsPer10Days = 1.0f;

        public int secondsUntilExitMapPossible = 120;
        public bool allowExitMapBeforeWin = false;

        public bool showDebugLogMessages = false;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref enemyFactionMultiplier, "enemyFactionMultiplier", 2.0f);
            Scribe_Values.Look(ref manhunterPackMultiplier, "manhunterPackMultiplier", 2.0f);
            Scribe_Values.Look(ref caravanVisibilityMultiplier, "caravanVisibilityMultiplier", 1.0f);

            Scribe_Values.Look(ref tempMapMultiplier, "tempMapMultiplier", 2.0f);
            Scribe_Values.Look(ref tempMapAdditionalAvgRaidsPer10Days, "tempMapAdditionalAvgRaidsPer10Days", 1.0f);

            Scribe_Values.Look(ref secondsUntilExitMapPossible, "secondsUntilExitMapPossible", 120);
            Scribe_Values.Look(ref allowExitMapBeforeWin, "allowExitMapBeforeWin", false);
            Scribe_Values.Look(ref showDebugLogMessages, "showDebugLogMessages", false);

            Scribe_Values.Look(ref usePlayerMainColonyThreat, "usePlayerMainColonyThreat", false);
            Scribe_Values.Look(ref enemyFactionPlayerMainColonyThreatMultiplier, "enemyFactionPlayerMainColonyThreatMultiplier", 0.1f);
            Scribe_Values.Look(ref manhunterPlayerMainColonyThreatMultiplier, "manhunterPlayerMainColonyThreatMultiplier", 0.1f);
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();

            listing_Standard.Begin(inRect);

            listing_Standard.Label("CM_Less_Shitty_Ambush_SettingEnemyFactionMultiplierLabel".Translate());
            listing_Standard.Label(enemyFactionMultiplier.ToString());
            enemyFactionMultiplier = listing_Standard.Slider(enemyFactionMultiplier, 0.1f, maxEnemyFactionMultiplier);

            listing_Standard.Label("CM_Less_Shitty_Ambush_SettingManhunterPackMultiplierLabel".Translate());
            listing_Standard.Label(manhunterPackMultiplier.ToString());
            manhunterPackMultiplier = listing_Standard.Slider(manhunterPackMultiplier, 0.1f, maxManhunterPackMultiplier);

            listing_Standard.Label("CM_Less_Shitty_Ambush_VisibilityMultiplier".Translate(), 
                -1, 
                "CM_Less_Shitty_Ambush_VisibilityMultiplierTooltip".Translate()
            );
            listing_Standard.Label(caravanVisibilityMultiplier.ToString());
            caravanVisibilityMultiplier = listing_Standard.Slider(caravanVisibilityMultiplier, 0.5f, 10.0f);

            listing_Standard.CheckboxLabeled(
                "CM_Less_Shitty_Ambush_SettingUsePlayerMainColonyThreat".Translate(), 
                ref usePlayerMainColonyThreat,
                "CM_Less_Shitty_Ambush_SettingUsePlayerMainColonyThreatTooltip".Translate()
            );
            
            listing_Standard.Label("CM_Less_Shitty_Ambush_SettingUsePlayerMainColonyThreatManhunterPackPercentageLabel".Translate());
            listing_Standard.Label(enemyFactionPlayerMainColonyThreatMultiplier.ToString());
            enemyFactionPlayerMainColonyThreatMultiplier = listing_Standard.Slider(enemyFactionPlayerMainColonyThreatMultiplier, 0f, 1.0f);
            
            listing_Standard.Label("CM_Less_Shitty_Ambush_SettingUsePlayerMainColonyThreatEnemyFactionPercentageLabel".Translate());
            listing_Standard.Label(manhunterPlayerMainColonyThreatMultiplier.ToString());
            manhunterPlayerMainColonyThreatMultiplier = listing_Standard.Slider(manhunterPlayerMainColonyThreatMultiplier, 0f, 1.0f);

            listing_Standard.Label("CM_Less_Shitty_Ambush_SettingTempMapMultiplierLabel".Translate());
            listing_Standard.Label(tempMapMultiplier.ToString());
            tempMapMultiplier = listing_Standard.Slider(tempMapMultiplier, 0.1f, maxTempMapMultiplier);

            listing_Standard.Label("CM_Less_Shitty_Ambush_SettingTempMapAdditionalAvgRaidsPer10DaysLabel".Translate(), 
                -1,
                "CM_Less_Shitty_Ambush_SettingTempMapAdditionalAvgRaidsPer10DaysLabelTooltip".Translate()
                );
            listing_Standard.Label(tempMapAdditionalAvgRaidsPer10Days.ToString());
            tempMapAdditionalAvgRaidsPer10Days = listing_Standard.Slider(tempMapAdditionalAvgRaidsPer10Days, 0f, 100.0f);

            listing_Standard.CheckboxLabeled("CM_Less_Shitty_Ambush_SettingExitMapBeforeWinLabel".Translate(), ref allowExitMapBeforeWin);

            if (allowExitMapBeforeWin)
            {
                listing_Standard.Label("CM_Less_Shitty_Ambush_SettingMapExitGridAvailableTimeLabel".Translate());
                listing_Standard.Label(secondsUntilExitMapPossible.ToString());
                secondsUntilExitMapPossible = (int)listing_Standard.Slider(secondsUntilExitMapPossible, 0, maxSecondsUntilExitMapPossible);
            }

            listing_Standard.CheckboxLabeled("CM_Less_Shitty_Ambush_SettingDebugLogMessagesLabel".Translate(), ref showDebugLogMessages);

            listing_Standard.End();
        }

        public void UpdateSettings()
        {
        }
    }
}
