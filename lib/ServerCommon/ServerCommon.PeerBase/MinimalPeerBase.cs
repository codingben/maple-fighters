using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerBase
{
    /// <inheritdoc />
    /// <summary>
    /// A base implementation for the peer.
    /// </summary>
    public abstract class MinimalPeerBase : IPeerBase
    {
        protected IClientPeer Peer { get; private set; }

        protected int PeerId => ProvidePeerId();

        public void Connected(IClientPeer peer)
        {
            Peer = peer;

            OnConnected();
            SubscribeToDisconnectionNotifier();
        }

        protected virtual void OnConnected()
        {
            LogUtils.Log(
                $"A new peer {PeerId} has been connected to the server.");
        }

        protected virtual void OnDisconnected(
            DisconnectReason reason,
            string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            LogUtils.Log(
                $"The peer {PeerId} has been disconnected from the server.");
        }

        private void SubscribeToDisconnectionNotifier()
        {
            Peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            Peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        protected abstract int ProvidePeerId();
    }
}