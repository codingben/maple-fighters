using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using ServerCommunicationInterfaces;
using Shared.Communication.Common.Peer;

namespace ServerApplication.Common.ApplicationBase
{
    /// <summary>
    /// Every server application should inherit from this base class to initialize a server application.
    /// </summary>
    public abstract class Application
    {
        public abstract void OnConnected(IClientPeer clientPeer);

        public abstract void Initialize();

        public void Dispose()
        {
            ServerComponents.Container.GetComponent(out PeerContainer peerContainer);
            peerContainer.Dispose();
        }

        protected void AddCommonComponents()
        {
            ServerComponents.Container.AddComponent(new RandomNumberGenerator());
            ServerComponents.Container.AddComponent(new IdGenerator());
            ServerComponents.Container.AddComponent(new PeerContainer());
        }

        protected void WrapClientPeer(ClientPeer<IClientPeer> clientPeer)
        {
            ServerComponents.Container.GetComponent(out IdGenerator idGenerator);
            ServerComponents.Container.GetComponent(out PeerContainer peerContainer);

            var peerId = idGenerator.GenerateId();
            peerContainer.AddPeerLogic(peerId, clientPeer);
        }
    }
}