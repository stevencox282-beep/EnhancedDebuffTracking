using HarmonyLib;
using Il2Cpp;
using Il2CppPantheonPersist;

namespace EnhancedDebuffTracking.Hooks;

[HarmonyPatch(typeof(EntityClientMessaging.Logic), nameof(EntityClientMessaging.Logic.SendChatMessage), typeof(string), typeof(ChatChannelType))]
public class SendChatMessageHook
{
    private static bool Prefix(EntityClientMessaging.Logic __instance, string message, ChatChannelType channel)
    {
        if (Globals.PlayerIsLoaded == true)
        {
            if (message == "/showdebuff")
            {
                // The clock is already loaded and running via another hook, all we have to do is show it
                ModMain.ShowDebuffPanel();
                return false;
            }

            if (message == "/hidedebuff")
            {
                // The clock is already loaded and running via another hook, all we have to do is show it
                ModMain.HideDebuffPanel();
                return false;
            }
        }
        return true;
    }
}