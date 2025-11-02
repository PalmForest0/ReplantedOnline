using HarmonyLib;
using Il2CppSource.DataModels;
using ReplantedOnline.Items.Enums;
using ReplantedOnline.Modules;
using ReplantedOnline.Network.Online;

namespace ReplantedOnline.Patches.Versus;

[HarmonyPatch]
internal static class VersusModePatch
{
    // TODO: Classes to look at to sync actions, Ahhhhhh I HATE Il2Cpp :(
    // VersusMode : ReloadedMode
    // VersusDataModel : DisposableObjectModel
    // VersusPlayerModel : DisposableObjectModel
    // VersusChooserSwapBinder : Binder
    // VersusWinDataResetActivity : InjectableActivity
    // Board : Widget
    // GameplayActivity : InjectableActivity
    // SeedChooserScreen : Widget

    [HarmonyPatch(typeof(VersusPlayerModel), nameof(VersusPlayerModel.Confirm))]
    [HarmonyPostfix]
    internal static void Confirm_Postfix(VersusPlayerModel __instance)
    {
        if (!NetLobby.AmLobbyHost()) return;

        if (Instances.GameplayActivity.VersusMode.PlantPlayerIndex == 0)
        {
            NetLobby.LobbyData.UpdateGameState(GameState.HostChoosePlants);
        }
        else
        {
            NetLobby.LobbyData.UpdateGameState(GameState.HostChooseZombie);
        }
    }

    [HarmonyPatch(typeof(VersusPlayerModel), nameof(VersusPlayerModel.Cancel))]
    [HarmonyPostfix]
    internal static void Cancel_Postfix(VersusPlayerModel __instance)
    {
        if (!NetLobby.AmLobbyHost()) return;

        NetLobby.LobbyData.UpdateGameState(GameState.Lobby);
    }

    [HarmonyPatch(typeof(VersusPlayerModel), nameof(VersusPlayerModel.Move))]
    [HarmonyPostfix]
    internal static void Move_Postfix(VersusPlayerModel __instance, float x)
    {
    }
}
