using Il2CppSteamworks;
using ReplantedOnline.Network;
using ReplantedOnline.Network.Online;

namespace ReplantedOnline.Helper;

/// <summary>
/// Provides extension methods for Steamworks types to simplify common operations
/// and improve code readability throughout the ReplantedOnline mod.
/// </summary>
internal static class SteamNetExtensions
{
    /// <summary>
    /// Retrieves a SteamNetClient instance by Steam ID from the current lobby.
    /// </summary>
    /// <param name="steamId">The Steam ID to search for.</param>
    /// <returns>The SteamNetClient instance if found in the current lobby, otherwise null.</returns>
    internal static SteamNetClient GetNetClient(this SteamId steamId)
    {
        if (NetLobby.LobbyData?.AllClients.TryGetValue(steamId, out var client) == true)
        {
            return client;
        }

        return default;
    }

    /// <summary>
    /// Checks if a Steam ID is banned from the current lobby.
    /// </summary>
    /// <param name="steamId">The Steam ID to check for ban status.</param>
    /// <returns>True if the Steam ID is banned from the current lobby, false otherwise.</returns>
    internal static bool Banned(this SteamId steamId)
    {
        return NetLobby.LobbyData?.Banned.Contains(steamId) == true;
    }

    /// <summary>
    /// Checks if a SteamNetClient is banned from the current lobby.
    /// </summary>
    /// <param name="steamNet">The SteamNetClient to check for ban status.</param>
    /// <returns>True if the SteamNetClient is banned from the current lobby, false otherwise.</returns>
    internal static bool Banned(this SteamNetClient steamNet)
    {
        return NetLobby.LobbyData?.Banned.Contains(steamNet.SteamId) == true;
    }
}