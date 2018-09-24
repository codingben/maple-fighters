using Common.ComponentModel;
using Common.Components;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommon.Application.Components;
using ServerCommon.Configuration;
using ServerCommon.Logging;
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
        protected IComponentsProvider Components { get; }

        private readonly IFiberProvider fiberProvider;
        private readonly IServerConnector serverConnector;

        protected internal ServerApplicationBase(
            IFiberProvider fiberProvider,
            IServerConnector serverConnector)
        {
            this.fiberProvider = fiberProvider;
            this.serverConnector = serverConnector;

            LogUtils.Logger = new Logger();
            TimeProviders.DefaultTimeProvider = new TimeProvider();

            Components = new ComponentsProvider();
            ServerExposedComponents.SetProvider(Components.ProvideExposed());

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
            var idGenerator = Components.Get<IIdGenerator>();
            if (idGenerator == null)
            {
                idGenerator = Components.Add(new IdGenerator());
            }

            OnConnected(clientPeer, idGenerator.GenerateId());
        }

        protected virtual void OnStartup()
        {
            LogUtils.Log("An application has started.");
        }

        protected virtual void OnShutdown()
        {
            Components?.Dispose();

            LogUtils.Log("An application has been stopped.");
        }

        protected virtual void OnConnected(IClientPeer clientPeer, int peerId)
        {
            // TODO: Find a way to unsubscribe from this event
            clientPeer.PeerDisconnectionNotifier.Disconnected += (x, y) => OnDisconnected(peerId);

            LogUtils.Log(
                $"A new peer ({peerId}) has been connected to the server.");
        }

        private void OnDisconnected(int peerId)
        {
            UnwrapClientPeer(peerId);

            LogUtils.Log(
                $"The peer ({peerId}) has been disconnected from the server.");
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
        /// <typeparam name="TPeerLogic">The peer logic.</typeparam>
        /// <param name="clientPeer">The client peer.</param>
        /// <param name="peerId">Unique id of the peer.</param>
        /// <param name="peerLogic">The peer logic instance.</param>
        protected void WrapClientPeer<TPeerLogic>(
            IClientPeer clientPeer,
            int peerId,
            IPeerLogicBase peerLogic = null)
            where TPeerLogic : IPeerLogicBase, new()
        {
            if (peerLogic == null)
            {
                peerLogic = new TPeerLogic();
            }

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

        /// <summary>
        /// Removes the peer logic from the client peer.
        /// </summary>
        /// <param name="peerId">Unique id of the peer.</param>
        private void UnwrapClientPeer(int peerId)
        {
            var peersLogicProvider = Components.Get<IPeersLogicProvider>().AssertNotNull();
            peersLogicProvider.RemovePeerLogic(peerId);
        }
    }
}