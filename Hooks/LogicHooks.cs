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

[HarmonyPatch(typeof(Buffs.Logic), nameof(Buffs.Logic.Add), typeof(double), typeof(ActiveBuff), typeof(bool), typeof(bool), typeof(bool))]
public class BuffLogicAdd
{
    private static void Prefix(double time, ActiveBuff buff, bool putInBackground = false, bool isRefresh = false, bool isItemBuff = false)
    {
        ModMain.OnAddOrRefreshBuff(time, buff, putInBackground, isRefresh, isItemBuff);
    }
}

//[HarmonyPatch(typeof(Buffs.Logic), nameof(Buffs.Logic.Add), typeof(double), typeof(ActiveBuff.SerializableBuff), typeof(bool), typeof(bool), typeof(bool))]
//public class BuffLogicAdd
//{
    //private static void Prefix(double time, ref ActiveBuff.SerializableBuff buff, bool putInBackground = false, bool isRefresh = false, bool isItemBuff = false)
    //{
        //MelonLogger.Warning($"BuffLogicAdd 1 buff.BuffId.ToString() = {buff.BuffId.ToString()}, buff.InternalId.ToString() = {buff.InternalId.ToString()}");
        //MelonLogger.Warning($"BuffLogicAdd 1 buff.CasterId.ToString() = {buff.CasterId.ToString()}, buff.TargetId.ToString() = {buff.TargetId.ToString()}");
    //}
//}



[HarmonyPatch(typeof(Buffs.Logic), nameof(Buffs.Logic.Remove), typeof(double), typeof(ulong), typeof(bool), typeof(bool), typeof(bool), typeof(int))]
public class BuffLogicRemove
{
    private static void Prefix(double time, ulong internalBuffId, bool moveToBackground = false, bool removeAllStacks = true, bool isItemBuff = false, int stacksToRemove = 1)
    {
        MelonLogger.Warning($"BuffLogicRemove() internalBuffId = {internalBuffId}");
        //ModMain.OnRemoveBuff(time, buffData, moveToBackground, isRefresh);
    }
}