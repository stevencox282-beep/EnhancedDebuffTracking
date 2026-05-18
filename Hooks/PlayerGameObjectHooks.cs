using HarmonyLib;
using Il2Cpp;

namespace EnhancedDebuffTracking.Hooks;

// This Hook fires when your character enters in the world (after character selection) or change of zone
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
            return;
        }
    }
}

// This Hook fires when you character exits the game or exits a zone
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