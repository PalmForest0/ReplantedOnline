using HarmonyLib;
using Il2CppReloaded.Gameplay;
using ReplantedOnline.Network.Online;
using ReplantedOnline.Network.RPC.Handlers;

namespace ReplantedOnline.Patches.Versus.NetworkSync;

[HarmonyPatch]
internal static class BoardSyncPatch
{
    /// <summary>
    /// Prefix patch that intercepts the Board.AddSunMoney method call.
    /// Runs before the original method and can prevent it from executing.
    /// </summary>
    [HarmonyPatch(typeof(Board), nameof(Board.AddSunMoney))]
    [HarmonyPrefix]
    internal static bool AddSunMoney_Prefix(Board __instance, int theAmount, int playerIndex)
    {
        // Only handle network synchronization if we're in a multiplayer lobby
        if (NetLobby.AmInLobby())
        {
            if (playerIndex == ReplantedOnlineMod.Constants.OPPONENT_PLAYER_INDEX)
            {
                return false;
            }

            if (playerIndex == ReplantedOnlineMod.Constants.LOCAL_PLAYER_INDEX)
            {
                SetMoneyHandler.Send(__instance.mSunMoney[playerIndex].Amount + theAmount);
                return true;
            }
        }

        return true;
    }

    /// <summary>
    /// Prefix patch that intercepts the Board.TakeSunMoney method call.
    /// Runs before the original method and can prevent it from executing.
    /// </summary>
    [HarmonyPatch(typeof(Board), nameof(Board.TakeSunMoney))]
    [HarmonyPrefix]
    internal static bool TakeSunMoney_Prefix(Board __instance, int theAmount, int playerIndex)
    {
        // Only handle network synchronization if we're in a multiplayer lobby
        if (NetLobby.AmInLobby())
        {
            if (playerIndex == ReplantedOnlineMod.Constants.OPPONENT_PLAYER_INDEX)
            {
                return false;
            }

            if (playerIndex == ReplantedOnlineMod.Constants.LOCAL_PLAYER_INDEX)
            {
                SetMoneyHandler.Send(__instance.mSunMoney[playerIndex].Amount - theAmount);
                return true;
            }
        }

        return true;
    }
}