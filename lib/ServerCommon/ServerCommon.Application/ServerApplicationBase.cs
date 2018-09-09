using Common.ComponentModel;
using Common.Components;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommon.Application.Components;
using ServerCommon.PeerLogic;
using ServerCommon.PeerLogic.Components;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application
{
    public class ServerApplicationBase : IApplicationBase
    {
        protected IComponentsProvider Components { get; } = new ComponentsProvider();

        private readonly IFiberProvider fiberProvider;
        private readonly IServerConnector serverConnector;

        public ServerApplicationBase(
            ILogger logger, IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            LogUtils.Logger = logger;
            TimeProviders.DefaultTimeProvider = new TimeProvider();

            this.fiberProvider = fiberProvider;
            this.serverConnector = serverConnector;

            ServerExposedComponents.SetProvider(Components.ProvideExposed());
        }

        public void Startup()
        {
            OnStartup();
        }

        public void Shutdown()
        {
            OnShutdown();
        }

        public void Connected(IClientPeer clientPeer)
        {
            OnConnected(clientPeer);
        }

        protected virtual void OnStartup()
        {
            // Left blank intentionally
        }

        protected virtual void OnShutdown()
        {
            Components?.Dispose();
        }

        protected virtual void OnConnected(IClientPeer clientPeer)
        {
            // Left blank intentionally
        }

        private void AddCommonComponents()
        {
            Components.Add(new RandomNumberGenerator());
            IFiberStarter fiber = Components.Add(new FiberStarter(fiberProvider));
            var executor = new FiberCoroutinesExecutor(
                fiber.GetFiberStarter(), updateRateMilliseconds: 100);
            Components.Add(new CoroutinesManager(executor));
            Components.Add(new ServerConnectorProvider(serverConnector));
        }

        private void AddPeerRelatedComponents()
        {
            Components.Add(new IdGenerator());
            Components.Add(new PeersLogicProvider());
        }

        protected void WrapClientPeer(IClientPeer clientPeer, IPeerLogicBase peerLogic)
        {
            var idGenerator = Components.Get<IIdGenerator>().AssertNotNull();
            var id = idGenerator.GenerateId();

            IPeerLogicProvider peerLogicProvider =
                new PeerLogicProvider<IClientPeer>(clientPeer, id);
            peerLogicProvider.SetPeerLogic(peerLogic);

            var peersLogicProvider = Components.Get<IPeersLogicProvider>().AssertNotNull();
            peersLogicProvider.AddPeerLogic(peerLogicProvider);
        }
    }
}