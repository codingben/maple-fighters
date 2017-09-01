using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using ServerApplication.Common.Components.Coroutines;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.Peer;

namespace ServerApplication.Common.ApplicationBase
{
    /// <summary>
    /// Every server application should inherit from this base class to initialize a server application.
    /// </summary>
    public abstract class Application
    {
        private readonly IFiberProvider fiberProvider;
        private PeerContainer peerContainer;

        protected Application(IFiberProvider fiberProvider)
        {
            this.fiberProvider = fiberProvider;
            this.fiberProvider.GetFiber().Start();
        }

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
            ServerComponents.Container.AddComponent(new CoroutinesExecuter(new FiberCoroutinesExecuter(fiberProvider.GetFiber(), 100)));
        }

        protected void WrapClientPeer(IClientPeerWrapper<IClientPeer> clientPeer)
        {
            peerContainer.AddPeerLogic(clientPeer);
        }
    }
}