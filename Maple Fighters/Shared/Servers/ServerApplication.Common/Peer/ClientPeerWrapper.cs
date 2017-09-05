using System;
using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public sealed class ClientPeerWrapper<T> : IClientPeerWrapper<T>
        where T : class, IClientPeer
    {
        public int PeerId { get; }

        public T Peer { get; }
        public IPeerLogicBase PeerLogic { get; private set; }

        public event Action<DisconnectReason, string> Disconnected;

        public ClientPeerWrapper(T peer, int peerId)
        {
            Peer = peer;
            PeerId = peerId;

            Peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        public void SetPeerLogic<TPeerLogic>(TPeerLogic peerLogic)
            where TPeerLogic : IPeerLogicBase
        {
            Peer.Fiber.Enqueue(() =>
            {
                Peer.NetworkTrafficState = NetworkTrafficState.Paused;

                PeerLogic?.Dispose();

                PeerLogic = peerLogic;
                PeerLogic.Initialize(this, PeerId);
            });
        }

        public void Dispose()
        {
            Peer.Fiber.Enqueue(() =>
            {
                PeerLogic.Dispose();

                if (Peer.IsConnected)
                {
                    Peer.Disconnect();
                }

                Peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
            });
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            Disconnected?.Invoke(disconnectReason, s);

            Dispose();
        }
    }
}