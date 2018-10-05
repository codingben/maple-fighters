using Common.ComponentModel;
using Common.Components;
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
    public class InboundPeerBase : PeerBase
    {
        private IExposedComponentsProvider ServerComponents => 
            ServerExposedComponents.Provide();

        protected internal InboundPeerBase()
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
            where TPeerLogic : IPeerLogicBase, new()
        {
            if (peerLogic == null)
            {
                peerLogic = new TPeerLogic();
            }

            PeerLogicProvider.SetPeerLogic(peerLogic);

            var peersLogicsProvider = 
                ServerComponents.Get<IPeersLogicsProvider>().AssertNotNull();
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

        protected override int ProvidePeerId()
        {
            var idGenerator =
                ServerComponents.Get<IIdGenerator>().AssertNotNull();
            return idGenerator.GenerateId();
        }

        protected override IPeerLogicProvider ProvidePeerLogic()
        {
            return new PeerLogicProvider<IClientPeer>(
                (IClientPeer)Peer,
                PeerId);
        }
    }
}