using Common.ComponentModel;
using Common.Components;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommon.Application.Components;
using ServerCommon.Configuration;
using ServerCommon.PeerLogic;
using ServerCommon.PeerLogic.Components;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application
{
    /// <inheritdoc />
    /// <summary>
    /// A base for the server application to be initialized properly.
    /// </summary>
    public class ServerApplicationBase : IApplicationBase
    {
        protected IComponentsProvider Components { get; } = new ComponentsProvider();

        private readonly IFiberProvider fiberProvider;
        private readonly IServerConnector serverConnector;

        internal ServerApplicationBase(
            ILogger logger,
            IFiberProvider fiberProvider,
            IServerConnector serverConnector)
        {
            LogUtils.Logger = logger;
            TimeProviders.DefaultTimeProvider = new TimeProvider();

            this.fiberProvider = fiberProvider;
            this.serverConnector = serverConnector;

            var exposedComponents = Components.ProvideExposed();
            ServerExposedComponents.SetProvider(exposedComponents);
            ServerConfiguration.Setup();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IApplicationBase.Startup"/> for more information.
        /// </summary>
        public void Startup()
        {
            OnStartup();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IApplicationBase.Shutdown"/> for more information.
        /// </summary>
        public void Shutdown()
        {
            OnShutdown();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IApplicationBase.Connected"/> for more information.
        /// </summary>
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

        /// <summary>
        /// Adds common components:
        /// IdGenerator
        /// RandomNumberGenerator
        /// FiberStarter
        /// CoroutinesManager
        /// PeersLogicProvider
        /// </summary>
        protected void AddCommonComponents()
        {
            Components.Add(new IdGenerator());
            Components.Add(new RandomNumberGenerator());
            IFiberStarter fiber = Components.Add(new FiberStarter(fiberProvider));
            var executor = new FiberCoroutinesExecutor(
                fiber.GetFiberStarter(), updateRateMilliseconds: 100);
            Components.Add(new CoroutinesManager(executor));
            Components.Add(new PeersLogicProvider());
        }

        /// <summary>
        /// Adds S2S related components:
        /// ServerConnectorProvider
        /// </summary>
        protected void AddS2SRelatedComponents()
        {
            Components.Add(new ServerConnectorProvider(serverConnector));
        }

        /// <summary>
        /// Sets a peer logic for the client peer.
        /// </summary>
        /// <param name="clientPeer">The client peer.</param>
        /// <param name="peerLogic">The actual logic.</param>
        protected void WrapClientPeer(
            IClientPeer clientPeer,
            IPeerLogicBase peerLogic)
        {
            var idGenerator = Components.Get<IIdGenerator>();
            if (idGenerator == null)
            {
                idGenerator = Components.Add(new IdGenerator());
            }

            var peerId = idGenerator.GenerateId();

            IPeerLogicProvider peerLogicProvider =
                new PeerLogicProvider<IClientPeer>(clientPeer, peerId);
            peerLogicProvider.SetPeerLogic(peerLogic);

            var peersLogicProvider = Components.Get<IPeersLogicProvider>();
            if (peersLogicProvider == null)
            {
                peersLogicProvider = Components.Add(new PeersLogicProvider());
            }

            peersLogicProvider.AddPeerLogic(peerId, peerLogicProvider);
        }
    }
}