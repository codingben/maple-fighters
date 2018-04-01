using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common
{
    public sealed class ClientPeerWrapper : IClientPeerWrapper
    {
        public int PeerId { get; }
        public IClientPeer Peer { get; }
        public IPeerLogicBase PeerLogic { get; private set; }

        public ClientPeerWrapper(IClientPeer peer, int peerId)
        {
            Peer = peer;
            PeerId = peerId;

            SubscribeToPeerDisconnectionNotifier();
        }

        private void SubscribeToPeerDisconnectionNotifier()
        {
            Peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromPeerDisconnectionNotifier()
        {
            Peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        public void SetPeerLogic<TPeerLogic>(TPeerLogic peerLogic)
            where TPeerLogic : IPeerLogicBase
        {
            Peer.Fiber.Enqueue(() =>
            {
                Peer.NetworkTrafficState = NetworkTrafficState.Paused;

                PeerLogic?.Dispose();
                PeerLogic = peerLogic;
                PeerLogic.Initialize(this);

                Peer.NetworkTrafficState = NetworkTrafficState.Flowing;
            });
        }

        public void Dispose()
        {
            Peer.Fiber.Enqueue(() =>
            {
                PeerLogic?.Dispose();
            });

            UnsubscribeFromPeerDisconnectionNotifier();
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            Dispose();
        }
    }
}