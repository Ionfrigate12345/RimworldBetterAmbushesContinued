using System.Text;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace CM_Less_Shitty_Ambush
{
    [StaticConstructorOnStartup]
    public static class CaravanVisibility_Patches
    {
        [HarmonyPatch(typeof(CaravanVisibilityCalculator))]
        [HarmonyPatch(nameof(CaravanVisibilityCalculator.Visibility), new[] { typeof(float), typeof(bool), typeof(StringBuilder) })]
        public static class CaravanVisibility_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(
                float bodySizeSum,
                bool caravanMovingNow,
                StringBuilder explanation,
                ref float __result
            )
            {
                float originalVisibility = __result;
                float newVisibility = originalVisibility * LessShittyAmbushMod.settings.caravanVisibilityMultiplier;
                __result = newVisibility;

                /*if (explanation != null)
                {
                    explanation.AppendLine();
                    explanation.Append($"<color=#ffcc00>YourMod changed visibility from {originalVisibility:F2} to {newVisibility:F2}</color>");
                }*/
            }
        }
    }
}
