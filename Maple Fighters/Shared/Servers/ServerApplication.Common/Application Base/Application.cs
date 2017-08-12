using System.Collections.Generic;
using ServerCommunicationInterfaces;
using Shared.Communication.Common.Peer;

namespace ServerApplication.Common.ApplicationBase
{
    /// <summary>
    /// Every server application should inherit from this base class to start a server application.
    /// </summary>
    public abstract class Application
    {
        private readonly List<ClientPeer<IClientPeer>> peerLogics = new List<ClientPeer<IClientPeer>>();

        public abstract void OnConnected(IClientPeer clientPeer);

        public abstract void Initialize();

        public void Dispose()
        {
            foreach (var peer in peerLogics)
            {
                peer.Dispose();
            }

            peerLogics.Clear();

            Disposed();
        }

        protected abstract void Disposed();

        protected void CreateNewPeerLogic(ClientPeer<IClientPeer> peerLogic)
        {
            peerLogic.Disconnected += () => PeerDisconnected(peerLogic);

            peerLogics.Add(peerLogic);
        }

        private void PeerDisconnected(ClientPeer<IClientPeer> peerLogic)
        {
            peerLogics.Remove(peerLogic);
        }
    }
}