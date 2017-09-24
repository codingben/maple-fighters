using CommonTools.Coroutines;
using CommonTools.Log;
using ServerApplication.Common.Components;
using ServerApplication.Common.Components.Coroutines;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.PeerLogic;

namespace ServerApplication.Common.ApplicationBase
{
    /// <summary>
    /// A base class to startup a server application
    /// </summary>
    public abstract class ApplicationBase : IApplication
    {
        private PeerContainer peerContainer;
        private readonly IFiberProvider fiberProvider;

        protected ApplicationBase(IFiberProvider fiberProvider)
        {
            this.fiberProvider = fiberProvider;
        }

        public abstract void OnConnected(IClientPeer clientPeer);

        public virtual void Startup()
        {
            Server.Entity.Container.AddComponent(new PeerContainer());

            peerContainer = Server.Entity.Container.GetComponent<PeerContainer>().AssertNotNull();

            LogUtils.Log(MessageBuilder.Trace("An application has started."));
        }

        public virtual void Shutdown()
        {
            Server.Entity.Dispose();

            peerContainer.DisconnectAllPeers();

            LogUtils.Log(MessageBuilder.Trace("An application has been stopped."));
        }

        protected void AddCommonComponents()
        {
            TimeProviders.DefaultTimeProvider = new TimeProvider();

            Server.Entity.Container.AddComponent(new RandomNumberGenerator());
            Server.Entity.Container.AddComponent(new IdGenerator());
            var fiber = Server.Entity.Container.AddComponent(new FiberProvider(fiberProvider)).AssertNotNull();
            Server.Entity.Container.AddComponent(new CoroutinesExecutor(new FiberCoroutinesExecutor(fiber.GetFiberStarter(), 100)));
        }

        protected void WrapClientPeer(IClientPeer clientPeer, IPeerLogicBase peerLogic)
        {
            var idGenerator = Server.Entity.Container.GetComponent<IdGenerator>().AssertNotNull();
            var peerId = idGenerator.GenerateId();

            var clientPeerWrapper = new ClientPeerWrapper<IClientPeer>(clientPeer, peerId);
            clientPeerWrapper.SetPeerLogic(peerLogic);

            peerContainer.AddPeerLogic(clientPeerWrapper);
        }
    }
}