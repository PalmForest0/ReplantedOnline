using ReplantedOnline.Items.Attributes;
using ReplantedOnline.Items.Enums;
using ReplantedOnline.Modules;
using ReplantedOnline.Network.Online;
using ReplantedOnline.Network.Packet;

namespace ReplantedOnline.Network.RPC.Handlers;

[RegisterRPCHandler]
internal class SetMoneyHandler : RPCHandler
{
    /// <inheritdoc/>
    internal sealed override RpcType Rpc => RpcType.SetMoney;

    internal static void Send(int amount)
    {
        var packetWriter = PacketWriter.Get();
        packetWriter.WriteInt(amount);
        NetworkDispatcher.SendRpc(RpcType.SetMoney, packetWriter);
    }

    /// <inheritdoc/>
    internal sealed override void Handle(SteamNetClient sender, PacketReader packetReader)
    {
        var amount = packetReader.ReadInt();
        Instances.GameplayActivity.Board.mSunMoney[ReplantedOnlineMod.Constants.OPPONENT_PLAYER_INDEX].Amount = amount;
    }
}
