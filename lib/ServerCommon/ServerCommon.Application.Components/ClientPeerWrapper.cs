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
        private IDisposable peerLogic;
        private IClientPeerContainer clientPeerContainer;

        public ClientPeerWrapper(int id, IClientPeer peer, IDisposable peerLogic)
        {
            this.id = id;
            this.peer = peer;
            this.peerLogic = peerLogic;

            SubscribeToPeerDisconnectionNotifier();
        }

        public void SetClientPeerContainer(IClientPeerContainer clientPeerContainer)
        {
            this.clientPeerContainer = clientPeerContainer;
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