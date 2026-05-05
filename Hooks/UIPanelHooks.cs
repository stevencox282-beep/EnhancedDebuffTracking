using HarmonyLib;
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

//[HarmonyPatch(typeof(UIBuffBar), nameof(UIBuffBar.OnAddOrRefreshBuff))]
//public class UIBuffBarOnAddOrRefreshBuff
//{
//    private static void Postfix(double time, ActiveBuff buff, bool inBackground, bool isRefresh, bool isItemBuff)
//    {
//        // This has been replaced by Buff.Logic.Add in LogicHooks
//        //ModMain.OnAddOrRefreshBuff(time, buff, inBackground, isRefresh, isItemBuff);
//    }
//}

[HarmonyPatch(typeof(UIBuffBar), nameof(UIBuffBar.OnRemoveBuff))]
public class UIBuffBarOnRemoveBuff
{
    private static void Postfix(double time, ActiveBuff buff, bool moveToBackground, bool isRefresh)
    {
        // This has been replaced by Buff.Logic.Remove in LogicHooks
        ModMain.OnRemoveBuff(time, buff, moveToBackground, isRefresh);
    }
}
