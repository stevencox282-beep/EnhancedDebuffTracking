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
            if (message == "/showdebuffs")
            {
                // The clock is already loaded and running via another hook, all we have to do is show it
                Globals.ShowDebuffPanel = true;
                ModMain.ShowDebuffPanel();
                return false;
            }

            if (message == "/hidedebuffs")
            {
                // The clock is already loaded and running via another hook, all we have to do is show it
                Globals.ShowDebuffPanel = false;
                ModMain.HideDebuffPanel();
                return false;
            }

            if (message.Contains("/setdebuffrows"))
            {
                ModMain.SetNumDebuffRows(message);
                return false;
            }
            
        }
        return true;
    }
}