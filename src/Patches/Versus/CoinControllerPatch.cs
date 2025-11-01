using HarmonyLib;
using Il2CppReloaded.Gameplay;
using Il2CppReloaded.TreeStateActivities;
using ReplantedOnline.Network;
using ReplantedOnline.Network.Online;

namespace ReplantedOnline.Patches.Versus;

[HarmonyPatch]
internal class CoinControllerPatch
{
    [HarmonyPatch(typeof(GameplayActivity), nameof(GameplayActivity.CreateCoinController))]
    [HarmonyPrefix]
    internal static bool CreateCoinController_Prefix(Coin coin)
    {
        // Only apply restrictions when the player is in an online lobby (versus mode)
        if (NetLobby.AmInLobby())
        {
            // Check if the local player is on the zombie team
            if (SteamNetClient.LocalClient.AmZombieSide())
            {
                // Zombie players should not receive sun-related coins (plant team currency)
                if (coin.mType is CoinType.Sun or CoinType.LargeSun or CoinType.SmallSun or CoinType.DoubleSun)
                {
                    // Prevent creation of sun coins for zombie players
                    return false;
                }
            }
            else // Player is on the plant team
            {
                // Plant players should not receive brain coins (zombie team currency)
                if (coin.mType is CoinType.Brain)
                {
                    // Prevent creation of brain coins for plant players
                    return false;
                }
            }
        }

        return true;
    }
}