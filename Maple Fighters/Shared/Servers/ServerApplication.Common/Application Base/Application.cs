using CommonTools.Coroutines;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using ServerApplication.Common.Components.Coroutines;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.Peer;

namespace ServerApplication.Common.ApplicationBase
{
    /// <summary>
    /// A base class to startup a server application
    /// </summary>
    public abstract class Application : IApplication
    {
        private PeerContainer peerContainer;

        private readonly IFiberProvider fiberProvider;

        protected Application(IFiberProvider fiberProvider)
        {
            this.fiberProvider = fiberProvider;
        }

        public abstract void OnConnected(IClientPeer clientPeer);

        public virtual void Startup()
        {
            peerContainer = ServerComponents.Container.AddComponent(new PeerContainer());
        }

        public virtual void Shutdown()
        {
            peerContainer.Dispose();
        }

        protected void AddCommonComponents()
        {
            TimeProviders.DefaultTimeProvider = new TimeProvider();

            ServerComponents.Container.AddComponent(new RandomNumberGenerator());
            ServerComponents.Container.AddComponent(new IdGenerator());
            ServerComponents.Container.AddComponent(new CoroutinesExecutor(new FiberCoroutinesExecutor(FiberStarter(), 100)));
        }

        protected void WrapClientPeer(IClientPeerWrapper<IClientPeer> clientPeer)
        {
            peerContainer.AddPeerLogic(clientPeer);
        }

        private IFiber FiberStarter()
        {
            var fiber = fiberProvider.GetFiber();
            fiber.Start();
            return fiber;
        }
    }
}