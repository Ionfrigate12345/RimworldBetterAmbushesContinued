using CM_Less_Shitty_Ambush.Global;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CM_Less_Shitty_Ambush
{
    [StaticConstructorOnStartup]
    public static class IncidentWorker_CaravanDemand_Patches
    {
        [HarmonyPatch(typeof(IncidentWorker_CaravanDemand))]
        [HarmonyPatch("TryExecuteWorker", MethodType.Normal)]
        public static class IncidentWorker_CaravanDemand_TryExecuteWorker
        {
            [HarmonyPrefix]
            public static void Prefix(IncidentWorker_CaravanDemand __instance, ref IncidentParms parms)
            {
                float newPoints = parms.points * LessShittyAmbushMod.settings.enemyFactionMultiplier;
                Logger.MessageFormat(__instance, "(Less Shitty Ambush)Muliplying enemy faction points: {0} * {1} = {2}", parms.points, LessShittyAmbushMod.settings.enemyFactionMultiplier, newPoints);

                if (LessShittyAmbushMod.settings.usePlayerMainColonyThreat)
                {
                    int colonyThreatAmbushPoints = Utils.GetAmbushThreatPointsByPlayerMainColonyMapWealth(LessShittyAmbushMod.settings.enemyFactionPlayerMainColonyThreatMultiplier);
                    newPoints += colonyThreatAmbushPoints;
                    Logger.MessageFormat(__instance, "(Less Shitty Ambush)Adding player colony points as additional ambush strength: {0}", colonyThreatAmbushPoints);
                }

                parms.points = newPoints;
            }
        }
    }
}
