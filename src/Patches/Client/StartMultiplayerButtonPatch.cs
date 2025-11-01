using HarmonyLib;
using Il2CppSource.Binders;
using ReplantedOnline.Network.Online;

namespace ReplantedOnline.Patches.Client;

[HarmonyPatch]
internal static class StartMultiplayerButtonPatch
{
    [HarmonyPatch(typeof(StartMultiplayerButton), nameof(StartMultiplayerButton._onButtonClicked))]
    [HarmonyPrefix]
    internal static bool OnButtonClicked_Prefix(StartMultiplayerButton __instance)
    {
        if (__instance.gameObject.name != "CoopVS_VS_Button") return true;

        // Intercept multiplayer button click - create our online lobby instead of default behavior
        NetLobby.CreateLobby();

        // Skip original method to prevent default multiplayer from starting
        return false;
    }
}