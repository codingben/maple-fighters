using System;
using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application.Components
{
    public class ClientPeerWrapper : IPeerWrapper
    {
        public IMinimalPeer Peer => peer;

        private readonly int id;
        private readonly IClientPeer peer;
        private readonly IClientPeerContainer clientPeerContainer;
        private IDisposable peerLogic;

        public ClientPeerWrapper(
            int id,
            IClientPeer peer,
            IDisposable peerLogic,
            IClientPeerContainer clientPeerContainer)
        {
            this.id = id;
            this.peer = peer;
            this.peerLogic = peerLogic;
            this.clientPeerContainer = clientPeerContainer;

            SubscribeToPeerDisconnectionNotifier();
        }

        public void ChangePeerLogic(IDisposable newPeerLogic)
        {
            peerLogic = newPeerLogic;
        }

        public void Dispose()
        {
            peerLogic?.Dispose();
        }

        private void SubscribeToPeerDisconnectionNotifier()
        {
            peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromPeerDisconnectionNotifier()
        {
            peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        private void OnDisconnected(DisconnectReason reason, string details)
        {
            UnsubscribeFromPeerDisconnectionNotifier();

            // TODO: Find an alternative to this method
            clientPeerContainer?.Remove(id);
        }
    }
}