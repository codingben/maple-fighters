using System;
using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace Shared.Communication.Common.Peer
{
    public class ClientPeer<T> : IDisposable
        where T : IClientPeer
    {
        public event Action<DisconnectReason, string> Disconnected;

        public T Peer { get; }

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