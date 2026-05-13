using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Threading;
using Il2CppViNL;
using MelonLoader;

namespace EnhancedDebuffTracking.Hooks;

[HarmonyPatch(typeof(Buffs.Logic), nameof(Buffs.Logic.Add), typeof(double), typeof(ActiveBuff), typeof(bool), typeof(bool), typeof(bool))]
public class BuffLogicAdd
{
    private static void Prefix(double time, ActiveBuff buff, bool putInBackground = false, bool isRefresh = false, bool isItemBuff = false)
    {
        ModMain.OnAddOrRefreshBuff(time, buff, putInBackground, isRefresh, isItemBuff);
    }
}


[HarmonyPatch(typeof(Threat.Logic), nameof(Threat.Logic.SetThreat), typeof(IEntity), typeof(int))]
public class ThreatLogicAdd
{
    private static void Prefix(IEntity item, int threat)
    {
        MelonLogger.Warning($"ThreatLogicAdd() attacker.Threat = {item.Targets?.Offensive?.Nameplate?.nameText.text}");
        MelonLogger.Warning($"ThreatLogicAdd() attacker.Threat = {item.Threat}");
        
        //ModMain.OnAddOrRefreshBuff(time, buff, putInBackground, isRefresh, isItemBuff);
    }
}