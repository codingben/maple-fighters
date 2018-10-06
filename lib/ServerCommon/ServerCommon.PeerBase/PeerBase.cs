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
    /// A common peer implementation for the inbound communication. 
    /// </summary>
    public class PeerBase : MinimalPeerBase
    {
        private IExposedComponentsProvider ServerComponents => 
            ServerExposedComponents.Provide();

        private IPeerLogicBase<IClientPeer> PeerLogicBase { get; set; }

        protected internal PeerBase()
        {
            // Left blank intentionally
        }

        /// <summary>
        /// Sets a peer logic for the peer.
        /// </summary>
        /// <typeparam name="TPeerLogic">The peer logic.</typeparam>
        /// <param name="peerLogic">The peer logic instance.</param>
        protected void BindPeerLogic<TPeerLogic>(
            TPeerLogic peerLogic = default(TPeerLogic))
            where TPeerLogic : IInboundPeerLogicBase, new()
        {
            Peer.Fiber.Enqueue(() =>
            {
                if (peerLogic == null)
                {
                    peerLogic = new TPeerLogic();
                }

                UnbindPeerLogic();

                if (peerLogic is IPeerLogicBase<IClientPeer> @base)
                {
                    PeerLogicBase = @base;
                    PeerLogicBase.Setup(Peer, PeerId);
                }

                Peer.NetworkTrafficState = NetworkTrafficState.Flowing;

                var peersLogicsProvider = 
                    ServerComponents.Get<IPeersLogicsProvider>().AssertNotNull();
                peersLogicsProvider.AddPeerLogic(PeerId, peerLogic);
            });
        }

        /// <summary>
        /// Removes the peer logic from the client peer.
        /// </summary>
        protected void UnbindPeerLogic()
        {
            Peer.NetworkTrafficState = NetworkTrafficState.Paused;

            PeerLogicBase?.Dispose();

            var peersLogicsProvider = 
                ServerComponents.Get<IPeersLogicsProvider>().AssertNotNull();
            peersLogicsProvider.RemovePeerLogic(PeerId);
        }

        protected override int ProvidePeerId()
        {
            var idGenerator =
                ServerComponents.Get<IIdGenerator>().AssertNotNull();
            return idGenerator.GenerateId();
        }
    }
}