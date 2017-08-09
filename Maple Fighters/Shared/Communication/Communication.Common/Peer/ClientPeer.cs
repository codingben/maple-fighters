using System;
using ServerCommunicationInterfaces;

namespace Shared.Communication.Common.Peer
{
    public class ClientPeer<T> : IDisposable
        where T : IClientPeer
    {
        protected T Peer { get; private set; }

        protected ClientPeer(T peer)
        {
            Peer = peer;
        }

        public void Dispose()
        {
            if (Peer.IsConnected)
            {
                Peer.Disconnect();
            }
        }
    }
}