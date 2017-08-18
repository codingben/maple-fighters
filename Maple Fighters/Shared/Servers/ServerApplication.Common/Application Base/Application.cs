using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.Peer;

namespace ServerApplication.Common.ApplicationBase
{
    /// <summary>
    /// Every server application should inherit from this base class to initialize a server application.
    /// </summary>
    public abstract class Application
    {
        private PeerContainer peerContainer;

        public abstract void OnConnected(IClientPeer clientPeer);

        public abstract void Initialize();

        public void Dispose()
        {
            peerContainer.Dispose();
        }

        protected void AddCommonComponents()
        {
            peerContainer = ServerComponents.Container.AddComponent(new PeerContainer()) as PeerContainer;

            ServerComponents.Container.AddComponent(new RandomNumberGenerator());
            ServerComponents.Container.AddComponent(new IdGenerator());
        }

        protected void WrapClientPeer(ClientPeer<IClientPeer> clientPeer, int peerId)
        {
            peerContainer.AddPeerLogic(clientPeer, peerId);
        }
    }
}