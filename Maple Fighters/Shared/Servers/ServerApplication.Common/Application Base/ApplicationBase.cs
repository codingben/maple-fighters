using CommonTools.Coroutines;
using CommonTools.Log;
using PeerLogic.Common;
using ServerApplication.Common.Components;
using ServerApplication.Common.Components.Interfaces;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.ApplicationBase
{
    /// <summary>
    /// A base class to startup a server application.
    /// </summary>
    public class ApplicationBase : IApplicationBase
    {
        private readonly IFiberProvider fiberProvider;
        private readonly IServerConnector serverConnector;

        private IPeerContainer peerContainer;

        protected ApplicationBase(IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            this.fiberProvider = fiberProvider;
            this.serverConnector = serverConnector;
        }

        public virtual void OnConnected(IClientPeer clientPeer)
        {
            LogUtils.Log($"A new peer has been connected -> {clientPeer.ConnectionInformation.Ip}:{clientPeer.ConnectionInformation.Port}");
        }

        public virtual void Startup()
        {
            peerContainer = Server.Components.AddComponent(new PeerContainer());

            LogUtils.Log("An application has started.");
        }

        public virtual void Shutdown()
        {
            Server.Components.Dispose();

            LogUtils.Log("An application has been stopped.");
        }

        protected void AddCommonComponents()
        {
            TimeProviders.DefaultTimeProvider = new TimeProvider();

            Server.Components.AddComponent(new RandomNumberGenerator());
            Server.Components.AddComponent(new IdGenerator());
            var fiber = Server.Components.AddComponent(new FiberProvider(fiberProvider)).AssertNotNull();
            Server.Components.AddComponent(new CoroutinesExecutor(new FiberCoroutinesExecutor(fiber.GetFiberStarter(), 100)));
            Server.Components.AddComponent(new ServerConnectorProvider(serverConnector));
        }

        protected void WrapClientPeer(IClientPeer clientPeer, IPeerLogicBase peerLogic)
        {
            var idGenerator = Server.Components.GetComponent<IIdGenerator>().AssertNotNull();
            var peerId = idGenerator.GenerateId();

            var clientPeerWrapper = new ClientPeerWrapper(clientPeer, peerId);
            clientPeerWrapper.SetPeerLogic(peerLogic);

            peerContainer.AddPeerLogic(clientPeerWrapper);
        }
    }
}