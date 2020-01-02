using CommonCommunicationInterfaces;
using ServerCommon.PeerLogic;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerBase
{
    /// <inheritdoc />
    /// <summary>
    /// A common peer implementation for the inbound communication. 
    /// </summary>
    public class PeerBase : IPeerBase
    {
        private IClientPeer peer;
        private int peerId;
        private IPeerLogicBase<IClientPeer> peerLogicBase;

        public void Connected(IClientPeer peer, int peerId)
        {
            this.peer = peer;
            this.peerId = peerId;
        }

        /// <summary>
        /// Sets a peer logic for the peer.
        /// </summary>
        /// <typeparam name="TPeerLogic">The peer logic.</typeparam>
        /// <param name="peerLogic">The peer logic instance.</param>
        protected void BindPeerLogic<TPeerLogic>(TPeerLogic peerLogic)
            where TPeerLogic : IPeerLogicBase<IClientPeer>
        {
            peer.Fiber.Enqueue(() =>
            {
                peer.NetworkTrafficState = NetworkTrafficState.Paused;

                UnbindPeerLogic();

                peerLogicBase = peerLogic;
                peerLogicBase.Setup(peer, peerId);

                peer.NetworkTrafficState = NetworkTrafficState.Flowing;
            });
        }

        /// <summary>
        /// Get rid of peer logic.
        /// </summary>
        protected void UnbindPeerLogic()
        {
            peerLogicBase?.Dispose();
        }
    }
}