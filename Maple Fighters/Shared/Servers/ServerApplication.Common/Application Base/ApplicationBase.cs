using CommonTools.Coroutines;
using CommonTools.Log;
using Components.Common;
using Components.Common.Interfaces;
using PeerLogic.Common;
using PeerLogic.Common.Components;
using PeerLogic.Common.Components.Interfaces;
using ServerApplication.Common.Components;
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
            peerContainer = ServerComponents.AddComponent(new PeerContainer());

            LogUtils.Log("An application has started.");
        }

        public virtual void Shutdown()
        {
            ServerComponents.RemoveAllComponents();

            LogUtils.Log("An application has been stopped.");
        }

        protected void AddCommonComponents()
        {
            TimeProviders.DefaultTimeProvider = new TimeProvider();

            ServerComponents.AddComponent(new RandomNumberGenerator());
            ServerComponents.AddComponent(new IdGenerator());
            var fiber = ServerComponents.AddComponent(new FiberProvider(fiberProvider)).AssertNotNull();
            ServerComponents.AddComponent(new CoroutinesManager(new FiberCoroutinesExecutor(fiber.GetFiberStarter(), 100)));
            ServerComponents.AddComponent(new ServerConnectorProvider(serverConnector));
        }

        protected void WrapClientPeer(IClientPeer clientPeer, IPeerLogicBase peerLogic)
        {
            var idGenerator = ServerComponents.GetComponent<IIdGenerator>().AssertNotNull();
            var peerId = idGenerator.GenerateId();

            var clientPeerWrapper = new ClientPeerWrapper(clientPeer, peerId);
            clientPeerWrapper.SetPeerLogic(peerLogic);

            peerContainer.AddPeerLogic(clientPeerWrapper);
        }
    }
}