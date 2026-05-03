using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppViNL;
using MelonLoader;

namespace EnhancedDebuffTracking.Hooks;

[HarmonyPatch(typeof(Targets.Logic), nameof(Targets.Logic.SetOffensive))]
public class TargetSetOffensiveHook
{
    private static void Postfix(Targets.Logic __instance)
    {
        ModMain.OffensiveTargetSelected(__instance);
    }
}

[HarmonyPatch(typeof(UIBuffBar), nameof(UIBuffBar.OnAddOrRefreshBuff))]
public class UIBuffBarOnAddOrRefreshBuff
{
    private static void Postfix(double time, ActiveBuff buff, bool inBackground, bool isRefresh, bool isItemBuff)
    {
        ModMain.OnAddOrRefreshBuff(time, buff, inBackground, isRefresh, isItemBuff);
    }
}

[HarmonyPatch(typeof(UIBuffBar), nameof(UIBuffBar.OnRemoveBuff))]
public class UIBuffBarOnRemoveBuff
{
    private static void Postfix(double time, ActiveBuff buff, bool moveToBackground, bool isRefresh)
    {
        ModMain.OnRemoveBuff(time, buff, moveToBackground, isRefresh);
    }
}