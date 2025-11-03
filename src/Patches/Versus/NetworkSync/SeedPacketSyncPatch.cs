using HarmonyLib;
using Il2CppReloaded.Gameplay;
using Il2CppReloaded.TreeStateActivities;
using ReplantedOnline.Modules;
using static Il2CppReloaded.Constants;

namespace ReplantedOnline.Patches.Versus.NetworkSync;

[HarmonyPatch]
internal static class SeedPacketSyncPatch
{
    // Rework planting seeds to support RPCs
    // This actually took hours to find out what's doing what :(
    [HarmonyPatch(typeof(GameplayActivity), nameof(GameplayActivity.OnMouseDownBG))]
    [HarmonyPrefix]
    internal static bool Selected_Prefix(GameplayActivity __instance, int mouseButton, int playerIndex)
    {
        // Check if the player is currently holding a plant in their cursor
        if (__instance.Board.IsPlantInCursor(0))
        {
            // Get the mouse position and convert it to grid coordinates
            var pos = Instances.GameplayActivity.GetMousePosition();
            var gridX = Instances.GameplayActivity.Board.PixelToGridXKeepOnBoard(pos.x, pos.y);
            var gridY = Instances.GameplayActivity.Board.PixelToGridYKeepOnBoard(pos.x, pos.y);

            // Get the type of seed being planted
            var seedType = __instance.Board.GetSeedTypeInCursor(0);

            // Check if planting at this position is valid
            if (Instances.GameplayActivity.Board.CanPlantAt(gridX, gridY, seedType) == PlantingReason.Ok)
            {
                // Find the seed packet from the seed bank that matches the seed type
                var packet = __instance.Board.mSeedBank.SeedPackets.FirstOrDefault(packet => packet.mPacketType == seedType);

                // Get the cost of the seed and check if player has enough sun
                var cost = packet.GetCost();
                if (__instance.Board.CanTakeSunMoney(cost, 0))
                {
                    // Mark the packet as used and deduct the sun cost
                    packet.WasPlanted(0);
                    __instance.Board.TakeSunMoney(cost, 0);
                    __instance.Board.ClearCursor();

                    // Actually place the seed at the grid position
                    PlaceSeed(seedType, packet.mImitaterType, gridX, gridY);
                }

                // Return false to skip the original method since we've handled planting
                return false;
            }

            // If planting is not valid, play buzzer sound
            Instances.GameplayActivity.m_audioService.PlaySample(Sound.SOUND_BUZZER);

            // Return false to skip original method (invalid placement)
            return false;
        }

        // Return true to execute original method (no plant in cursor, normal behavior)
        return true;
    }

    /// <summary>
    /// Places a seed (plant or zombie) at the specified grid position
    /// </summary>
    /// <param name="seedType">Type of seed to plant</param>
    /// <param name="imitaterType">Imitater plant type if applicable</param>
    /// <param name="gridX">X grid coordinate</param>
    /// <param name="gridY">Y grid coordinate</param>
    /// <returns>The created game object (plant or zombie)</returns>
    internal static ReloadedObject PlaceSeed(SeedType seedType, SeedType imitaterType, int gridX, int gridY)
    {
        // Check if this is a zombie seed (from I, Zombie mode)
        if (Challenge.IsZombieSeedType(seedType))
        {
            // Play zombie placement sound
            Instances.GameplayActivity.m_audioService.PlaySample(Sound.SOUND_PLANT2);

            // Convert seed type to actual zombie type
            var type = Challenge.IZombieSeedTypeToZombieType(seedType);

            // Determine if this zombie should rise from the ground
            var rise = VersusMode.ZombieRisesFromGround(type) && type != ZombieType.Bungee;
            // Force X position for zombies that don't rise from ground
            var forceXPos = !VersusMode.ZombieRisesFromGround(type);

            // Add zombie to the board at the specified position
            var zombie = Instances.GameplayActivity.Board.AddZombieAtCell(type, forceXPos ? 9 : gridX, gridY);

            // If this zombie rises from ground, trigger the rising animation
            if (rise)
            {
                zombie.RiseFromGrave(gridX, gridY);
            }
            return zombie;
        }
        else
        {
            // This is a regular plant seed - play plant placement sound
            Instances.GameplayActivity.m_audioService.PlaySample(Sound.SOUND_PLANT);

            // Add plant to the board at the specified position
            return Instances.GameplayActivity.Board.AddPlant(gridX, gridY, seedType, imitaterType);
        }
    }
}