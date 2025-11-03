using HarmonyLib;
using Il2CppReloaded.Gameplay;
using ReplantedOnline.Network.Online;

namespace ReplantedOnline.Patches.Versus;

[HarmonyPatch]
internal class ZombiePatch
{
    // Fix Bungee spawning in a random position
    [HarmonyPatch(typeof(Zombie), nameof(Zombie.PickBungeeZombieTarget))]
    [HarmonyPrefix]
    internal static bool PickBungeeZombieTarget_Prefix()
    {
        if (NetLobby.AmInLobby())
        {
            return false;
        }

        return true;
    }
}
