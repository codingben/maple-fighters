using System;
using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.Peer
{
    public class ClientPeer<T> : IDisposable
        where T : IClientPeer
    {
        public T Peer { get; }

        public event Action<DisconnectReason, string> Disconnected;

        protected ClientPeer(T peer)
        {
            Peer = peer;

            Peer.PeerDisconnectionNotifier.Disconnected += PeerDisconnectionNotifier;
        }

        public void Dispose()
        {
            if (Peer.IsConnected)
            {
                Peer.Disconnect();
            }
        }

        private void PeerDisconnectionNotifier(DisconnectReason disconnectReason, string s)
        {
            Disconnected?.Invoke(disconnectReason, s);
        }
    }
}