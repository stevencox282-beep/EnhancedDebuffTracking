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

// What I need, but it fires every time any kind of buff / debuff fires not just on an enemy
[HarmonyPatch(typeof(UIBuffBar), nameof(UIBuffBar.OnAddOrRefreshBuff))]
public class UIBuffBarOnAddOrRefreshBuff
{
    private static void Postfix(double time, ActiveBuff buff, bool inBackground, bool isRefresh, bool isItemBuff)
    {
        ModMain.OnAddOrRefreshBuff(time, buff, inBackground, isRefresh, isItemBuff);
    }
}


// What I need, but it fires every time any kind of buff / debuff fires not just on an enemy
// Does this get fired when somebody else applies a debuff?
[HarmonyPatch(typeof(UIBuffBar), nameof(UIBuffBar.OnRemoveBuff))]
public class UIBuffBarOnRemoveBuff
{
    private static void Postfix(double time, ActiveBuff buff, bool moveToBackground, bool isRefresh)
    {
        ModMain.OnRemoveBuff(time, buff, moveToBackground, isRefresh);
    }
}