using EnhancedDebuffTracking;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace PantheonMetrics.Hooks;

public static class EntityStatusLogicHook
{
    private const float UpdateInterval = 0.75f; // Update interval in seconds
    private static float _timeSinceLastUpdate;

    // This HarmonyPatch fires every frame which is not optimal for our use but isDead ALWAYS returns false and isDeadOrNearDead NEVER fires
    [HarmonyPatch(typeof(EntityStatus.Logic), nameof(EntityStatus.Logic.IsAlive))]
    public class EntityStausIsDeadLogicHook
    {
        private static void Prefix(EntityStatus.Logic __instance)
        {
            if (__instance == null)
                return;

            // Throttle this so it doesnt fire every frame
            _timeSinceLastUpdate += UnityEngine.Time.deltaTime;
            if (_timeSinceLastUpdate >= UpdateInterval)
            {
                // Update this immediatly so we dont flood in here
                _timeSinceLastUpdate = 0f;
                //MelonLogger.Warning($"EntityStatusLogicHook() .Nameplate.IsAlive = {__instance.Entity.Nameplate.isDead} networkId = {__instance.Entity.NetworkId.ToString()}");
                ModMain.UpdateEnemyDeadStatus(__instance.Entity.NetworkId.ToString(), __instance.Entity.Nameplate.isDead);
            }
        }
    }
}
