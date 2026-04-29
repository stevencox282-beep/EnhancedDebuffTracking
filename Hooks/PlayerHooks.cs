using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNanoSockets;
using Il2CppPantheonPersist;
using Il2CppViNL;
using MelonLoader;

namespace EnhancedDebuffTracking.Hooks;

[HarmonyPatch(typeof(EntityPlayerGameObject), nameof(EntityPlayerGameObject.NetworkStart))]
public class PlayerNetworkStart
{
    private static void Postfix(EntityPlayerGameObject __instance)
    {
        // Fired in character select
        if (__instance.NetworkId.Value == 1)
        {
            return;
        }
        
        if (__instance.NetworkId.Value == EntityPlayerGameObject.LocalPlayerId.Value)
        {
            Globals.PlayerIsLoaded = true;
        }
    }
}

[HarmonyPatch(typeof(EntityPlayerGameObject), nameof(EntityPlayerGameObject.NetworkStop))]
public class PlayerNetworkStop
{
    private static void Prefix(EntityPlayerGameObject __instance)
    {
        // Fired in character select
        if (__instance.NetworkId.Value == 1)
        {
            return;
        }

        if (__instance.NetworkId.Value == EntityPlayerGameObject.LocalPlayerId.Value)
        {
            Globals.PlayerIsLoaded = false;
            return;
        }
    }
}

[HarmonyPatch(typeof(Targets.Logic), nameof(Targets.Logic.SetOffensive))]
public class TargetSetOffensiveHook
{
    private static void Postfix(Targets.Logic __instance)
    {
        // TODO - Why was this code here?
//        if (Globals.LocalPlayer?.Targets == __instance)
//        {
          // Do Something
          ModMain.OffensiveTargetSelected(__instance);
//        }
    }
}

[HarmonyPatch(typeof(Targets.Logic), nameof(Targets.Logic.SetDefensive))]
public class TargetSetDefensiveHook
{
    private static void Postfix(Targets.Logic __instance)
    {
        if (Globals.LocalPlayer?.Targets == __instance)
        {
            float percent = 0;
            if (__instance.Defensive != null)
            {
                var current = __instance.Defensive.Pools.GetCurrent(PoolType.Health);
                var max = __instance.Defensive.Pools.GetMax(PoolType.Health);
                percent = current / max * 100;
            }

            // Do Something
        }
    }
}


[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.UpdateGroupMembers))]
public class GroupAdd
{
    private static void Postfix(Il2CppReferenceArray<GroupMember> members, Il2CppStructArray<NetworkId> membersPetIds)
    {

    }

    private static void Pretfix(Il2CppReferenceArray<GroupMember> members, Il2CppStructArray<NetworkId> membersPetIds)
    {

    }
}

//[HarmonyPatch(typeof(Buffs.Logic), nameof(Buffs.Logic.Add))]
//public class UpdateBuffMembers
//{
//// TODO - There are multiple of these, find out if any of them are even relevant
//private static void Postfix(double time, ActiveBuff buff, bool putInBackground = false, bool isRefresh = false, bool isItemBuff = false)
//{
/// Do something        
//
//}
//}