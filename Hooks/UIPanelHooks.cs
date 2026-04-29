using HarmonyLib;
using MelonLoader;
using Il2Cpp;

namespace EnhancedDebuffTracking.Hooks;

[HarmonyPatch(typeof(UIWindowPanel), nameof(UIWindowPanel.Start))]
public class UIPanelHooks
{
    // Make changes to the layout of existing Panels using these hooks
    private static void Postfix(UIWindowPanel __instance)
    {
        // Monster Debuffs
        if (__instance.name == "Panel_OffensiveTarget")
        {
            ModMain.DebugOffensiveTargetPanel(__instance);
        }

        if (__instance.name == "Panel_DefensiveTarget")
        {
            ModMain.DebugDefensiveTargetPanel(__instance);
        }

        // Player Debuffs
        if (__instance.name == "Player")
        {
            //ModMain.PlayerDebug(__instance);

        }


        // Party Debuffs
        if (__instance.name == "GroupMembers")
        {
            // Do something in here to make the debuff icons bigger so people without bionic eyes can see them
            var buffBarPool = __instance.GetComponentsInChildren<UIBuffBar>(true);
            //ModMain.GroupPanelDebug(__instance);
        }
    }
}