using HarmonyLib;
using Il2Cpp;
using Il2CppReloaded.TreeStateActivities;
using ReplantedOnline.Network.Online;

namespace ReplantedOnline.Patches.Client;

[HarmonyPatch]
internal class PausePatch
{
    [HarmonyPatch(typeof(PauseGameActivity), nameof(PauseGameActivity.ActiveStarted))]
    [HarmonyPrefix]
    internal static bool PauseGameActivity_ActiveStarted_Prefix()
    {
        // Check if the player is currently in an online lobby
        if (NetLobby.AmInLobby())
        {
            // Skip the original pausing logic when in lobby
            // This prevents the game from being paused in multiplayer contexts
            return false;
        }

        // Allow normal pausing behavior when not in lobby
        return true;
    }

    [HarmonyPatch(typeof(PauseMusicActivity), nameof(PauseMusicActivity.ActiveStarted))]
    [HarmonyPrefix]
    internal static bool PauseMusicActivity_ActiveStarted_Prefix()
    {
        // Check if the player is currently in an online lobby
        if (NetLobby.AmInLobby())
        {
            // Skip the original music pausing logic when in lobby
            // This ensures background music continues playing during multiplayer sessions
            return false;
        }

        // Allow normal music pausing behavior when not in lobby
        return true;
    }
}