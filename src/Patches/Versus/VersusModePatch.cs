using HarmonyLib;

namespace ReplantedOnline.Patches.Versus;

[HarmonyPatch]
internal static class VersusModePatch
{
    // TODO: Classes to look at to sync actions, Ahhhhhh I HATE Il2Cpp :(
    // VersusMode : ReloadedMode
    // VersusDataModel : DisposableObjectModel
    // VersusPlayerModel : DisposableObjectModel
    // VersusChooserSwapBinder : Binder
    // Board : Widget
    // GameplayActivity : InjectableActivity
}
