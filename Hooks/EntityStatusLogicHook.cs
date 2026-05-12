using EnhancedDebuffTracking;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace PantheonMetrics.Hooks;

public static class EntityStatusLogicHook
{
    [HarmonyPatch(typeof(EntityStatus.Logic), nameof(EntityStatus.Logic.IsDead))]
    public class EntityStausIsDeadLogicHook
    {
        private static void Prefix(EntityStatus.Logic __instance)
        {
            if (__instance == null)
                return;

            //MelonLogger.Warning($"EntityStatusLogicHook() networkId = {__instance.Entity.NetworkId.ToString()}, nameText.text = {__instance.Entity.Nameplate.nameText.text}, status = {__instance.status}");
            //MelonLogger.Warning($"EntityStatusLogicHook() .Nameplate.isDead = {__instance.Entity.Nameplate.isDead} status = {__instance.status}");
            ModMain.MarkEnemyAsDead(__instance.Entity.NetworkId.ToString());
        }
    }
}
