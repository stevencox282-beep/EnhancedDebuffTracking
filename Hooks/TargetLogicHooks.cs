using HarmonyLib;
using Il2Cpp;

namespace EnhancedDebuffTracking.Hooks;

// User selects a new offensive target
[HarmonyPatch(typeof(Targets.Logic), nameof(Targets.Logic.SetOffensive))]
public class TargetSetOffensiveHook
{
    private static void Postfix(Targets.Logic __instance)
    {
        ModMain.OffensiveTargetSelected(__instance);
    }
}