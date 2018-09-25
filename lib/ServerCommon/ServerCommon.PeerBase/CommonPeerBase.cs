using Common.ComponentModel;
using Common.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommon.Application;
using ServerCommon.PeerLogic;
using ServerCommon.PeerLogic.Components;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerBase
{
    /// <inheritdoc />
    /// <summary>
    /// A common implementation for the client peer. 
    /// </summary>
    public class CommonPeerBase : IPeerBase
    {
        private IClientPeer Peer { get; set; }

        private int PeerId => ProvidePeerId();

        private IExposedComponentsProvider ServerComponents { get; }

        private IPeerLogicProvider PeerLogicProvider { get; set; }

        protected internal CommonPeerBase()
        {
            ServerComponents = ServerExposedComponents.Provide();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IPeerBase.Connected"/> for more information.
        /// </summary>
        public void Connected(IClientPeer peer)
        {
            Peer = peer;
            PeerLogicProvider = new PeerLogicProvider(peer, PeerId);

            OnConnected();

            SubscribeToDisconnectionNotifier();
        }

        protected virtual void OnConnected()
        {
            LogUtils.Log(
                $"A new peer ({PeerId}) has been connected to the server.");
        }

        protected virtual void OnDisconnected(
            DisconnectReason reason,
            string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            LogUtils.Log(
                $"The peer ({PeerId}) has been disconnected from the server.");
        }

        /// <summary>
        /// Sets a peer logic for the client peer.
        /// </summary>
        /// <typeparam name="TPeerLogic">The peer logic.</typeparam>
        /// <param name="peerLogic">The peer logic instance.</param>
        protected void BindPeerLogic<TPeerLogic>(
            TPeerLogic peerLogic = default(TPeerLogic))
            where TPeerLogic : IPeerLogicBase, new()
        {
            if (peerLogic == null)
            {
                peerLogic = new TPeerLogic();
            }

            PeerLogicProvider.SetPeerLogic(peerLogic);

            var peersLogicsProvider =
                ServerComponents.Get<IPeersLogicsProvider>();
            if (peersLogicsProvider == null)
            {
                peersLogicsProvider =
                    ServerComponents.Add(new PeersLogicsProvider());
            }

            peersLogicsProvider.AddPeerLogic(PeerId, peerLogic);
        }

        /// <summary>
        /// Removes the peer logic from the client peer.
        /// </summary>
        protected void UnbindPeerLogic()
        {
            PeerLogicProvider.UnsetPeerLogic();

            var peersLogicsProvider = 
                ServerComponents.Get<IPeersLogicsProvider>().AssertNotNull();
            peersLogicsProvider.RemovePeerLogic(PeerId);
        }

        private void SubscribeToDisconnectionNotifier()
        {
            Peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            Peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        private int ProvidePeerId()
        {
            var idGenerator = ServerComponents.Get<IIdGenerator>();
            if (idGenerator == null)
            {
                idGenerator = ServerComponents.Add(new IdGenerator());
            }

            return idGenerator.GenerateId();
        }
    }
}