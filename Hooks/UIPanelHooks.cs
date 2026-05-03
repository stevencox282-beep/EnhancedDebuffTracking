using HarmonyLib;
using MelonLoader;
using Il2Cpp;

namespace EnhancedDebuffTracking.Hooks;

[HarmonyPatch(typeof(UIWindowPanel), nameof(UIWindowPanel.Start))]
public class UIPanelHooksStart
{
    // Make changes to the layout of existing Panels using these hooks
    private static void Postfix(UIWindowPanel __instance)
    {
        // Monster Debuffs window can now be added to the UI
        if (__instance.name == "Panel_OffensiveTarget")
        {
            ModMain.AddDebuffPanelToUI(__instance);
        }
    }
}
