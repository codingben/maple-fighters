using System;
using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.Peer
{
    public abstract class ClientPeerWrapper<T> : IClientPeerWrapper<T>
        where T : IClientPeer
    {
        public T Peer { get; }
        public int PeerId { get; }

        public event Action<DisconnectReason, string> Disconnected;

        protected ClientPeerWrapper(T peer, int peerId)
        {
            Peer = peer;
            PeerId = peerId;

            Peer.PeerDisconnectionNotifier.Disconnected += OnPeerDisconnected;
        }

        protected virtual void OnPeerDisconnected(DisconnectReason disconnectReason, string s)
        {
            Disconnected?.Invoke(disconnectReason, s);
        }

        public void Dispose()
        {
            if (Peer.IsConnected)
            {
                Peer.Disconnect();
            }

            Peer.PeerDisconnectionNotifier.Disconnected -= OnPeerDisconnected;
        }

        public abstract void SendEvent<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;
    }
}