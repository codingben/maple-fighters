using System;
using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.Peer
{
    public class ClientPeer<T> : IDisposable
        where T : IClientPeer
    {
        public T Peer { get; }
        public int PeerId { get; }

        public event Action<DisconnectReason, string> Disconnected;

        protected ClientPeer(T peer, int peerId)
        {
            Peer = peer;
            PeerId = peerId;

            Peer.PeerDisconnectionNotifier.Disconnected += PeerDisconnectionNotifier;
        }

        public void Dispose()
        {
            if (Peer.IsConnected)
            {
                Peer.Disconnect();
            }
        }

        protected virtual void PeerDisconnectionNotifier(DisconnectReason disconnectReason, string s)
        {
            Disconnected?.Invoke(disconnectReason, s);
        }
    }
}