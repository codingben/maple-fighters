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
        private IPeerLogicBase<IClientPeer> PeerLogicBase { get; set; }

        private readonly IIdGenerator idGenerator;
        private readonly IPeersLogicsProvider peersLogicsProvider;

        protected PeerBase()
        {
            var components = ServerExposedComponents.Provide();
            idGenerator = components.Get<IIdGenerator>().AssertNotNull();
            peersLogicsProvider = 
                components.Get<IPeersLogicsProvider>().AssertNotNull();
        }

        /// <summary>
        /// Sets a peer logic for the peer.
        /// </summary>
        /// <typeparam name="TPeerLogic">The peer logic.</typeparam>
        /// <param name="peerLogic">The peer logic instance.</param>
        protected void BindPeerLogic<TPeerLogic>(TPeerLogic peerLogic = default)
            where TPeerLogic : IInboundPeerLogicBase, new()
        {
            Peer.Fiber.Enqueue(() =>
            {
                Peer.NetworkTrafficState = NetworkTrafficState.Paused;

                if (peerLogic == null)
                {
                    peerLogic = new TPeerLogic();
                }

                if (peerLogic is IPeerLogicBase<IClientPeer> @base)
                {
                    PeerLogicBase?.Dispose();

                    peersLogicsProvider?.RemovePeerLogic(PeerId);

                    PeerLogicBase = @base;
                    PeerLogicBase.Setup(Peer, PeerId);

                    peersLogicsProvider?.AddPeerLogic(PeerId, peerLogic);
                }

                Peer.NetworkTrafficState = NetworkTrafficState.Flowing;
            });
        }

        protected void UnbindPeerLogic()
        {
            PeerLogicBase?.Dispose();

            peersLogicsProvider?.RemovePeerLogic(PeerId);
        }

        protected override int ProvidePeerId()
        {
            return idGenerator.GenerateId();
        }
    }
}