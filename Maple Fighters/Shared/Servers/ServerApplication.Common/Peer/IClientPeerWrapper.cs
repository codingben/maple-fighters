using System;
using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public interface IClientPeerWrapper<out T> : IDisposable
        where T : IClientPeer
    {
        int PeerId { get; }

        T Peer { get; }
        IPeerLogicBase PeerLogic { get; }

        void SetPeerLogic<TPeerLogic>(TPeerLogic peerLogic)
            where TPeerLogic : IPeerLogicBase;

        event Action<DisconnectReason, string> Disconnected;
    }
}