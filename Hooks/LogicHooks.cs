using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppViNL;

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
public class TargetSetOffensiveHookUpdate
{
    private static void Postfix(double time, ActiveBuff buff, bool inBackground, bool isRefresh, bool isItemBuff)
    {
        ModMain.UpdateOffensiveTarget();
    }
}

[HarmonyPatch(typeof(Targets.Logic), nameof(Targets.Logic.SetDefensive))]
public class TargetSetDefensiveHook
{
    private static void Postfix(Targets.Logic __instance)
    {
      // Do Something
    }
}


[HarmonyPatch(typeof(Group.Logic), nameof(Group.Logic.UpdateGroupMembers))]
public class GroupAdd
{
    private static void Postfix(Il2CppReferenceArray<GroupMember> members, Il2CppStructArray<NetworkId> membersPetIds)
    {
        // Do Something
    }

    private static void Prefix(Il2CppReferenceArray<GroupMember> members, Il2CppStructArray<NetworkId> membersPetIds)
    {
        // Do Something
    }
}

//[HarmonyPatch(typeof(Buffs.Logic), nameof(Buffs.Logic.Add))]
//public class UpdateBuffMembers
//{
//    // TODO - There are multiple of these, find out if any of them are even relevant
//    private static void Postfix(double time, ActiveBuff buff, bool putInBackground = false, bool isRefresh = false, bool isItemBuff = false)
//    {
//        // Do something        
//        ModMain.handleBuffLogicAdd(time, buff, putInBackground, isRefresh, isItemBuff);
//    }
//}
//
//[HarmonyPatch(typeof(Buffs.Logic), nameof(Buffs.Logic.AddMyActiveBuff))]
//public class UpdateBuffMembersMyActiveBuff
//{
//    // TODO - There are multiple of these, find out if any of them are even relevant
//    private static void Postfix(double time, ActiveBuff buff)
//    {
//        // Do something        
//        ModMain.HandleBuffLogicMyActiveBuff(time, buff);
//    }
//}