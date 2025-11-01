using HarmonyLib;
using Il2CppReloaded.DataModels;
using Il2CppReloaded.TreeStateActivities;
using Il2CppTekly.Extensions.DataProviders;
using ReplantedOnline.Modules;

namespace ReplantedOnline.Patches;

[HarmonyPatch]
internal static class InstanceWrapperPatch
{
    [HarmonyPatch(typeof(UiDataProviderActivity), nameof(UiDataProviderActivity.LoadingStarted))]
    [HarmonyPostfix]
    internal static void LoadingStarted_Postfix(UiDataProviderActivity __instance)
    {
        // Only capture the data provider for the main gameplay activity
        if (__instance.gameObject.name == "GameplayActivity")
        {
            // Extract the GameplayDataProvider from the activity's providers
            GameplayDataProvider dataProvider = __instance.m_providers.First().Cast<GameplayDataProvider>();
            if (dataProvider != null)
            {
                InstanceWrapper<GameplayDataProvider>.Instance = dataProvider;
            }
        }
    }

    [HarmonyPatch(typeof(GameplayActivity), nameof(GameplayActivity.Awake))]
    [HarmonyPostfix]
    internal static void Awake_Postfix(GameplayActivity __instance)
    {
        InstanceWrapper<GameplayActivity>.Instance = __instance;
    }
}