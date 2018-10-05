using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic
{
    /// <inheritdoc />
    /// <summary>
    /// Provides the peer logic of the client peer.
    /// </summary>
    public class PeerLogicProvider<TPeer> : IPeerLogicProvider
        where TPeer : class, IMinimalPeer
    {
        private readonly TPeer peer;
        private readonly int peerId;

        private PeerLogicBase<TPeer> peerLogicBase;

        public PeerLogicProvider(TPeer peer, int peerId)
        {
            this.peer = peer;
            this.peerId = peerId;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IPeerLogicProvider.SetPeerLogic{T}"/> for more information.
        /// </summary>
        public void SetPeerLogic<TPeerLogic>(TPeerLogic peerLogic)
            where TPeerLogic : IPeerLogicBase
        {
            peer.Fiber.Enqueue(() =>
            {
                UnsetPeerLogic();

                peerLogicBase = peerLogic as PeerLogicBase<TPeer>;
                peerLogicBase?.Setup(peer, peerId);

                peer.NetworkTrafficState = NetworkTrafficState.Flowing;
            });
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IPeerLogicProvider.UnsetPeerLogic"/> for more information.
        /// </summary>
        public void UnsetPeerLogic()
        {
            peer.NetworkTrafficState = NetworkTrafficState.Paused;

            peerLogicBase?.Dispose();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IPeerLogicProvider.ProvidePeerLogic"/> for more information.
        /// </summary>
        public IPeerLogicBase ProvidePeerLogic()
        {
            if (peerLogicBase == null)
            {
                throw new PeerLogicException(
                    $"A peer with id {peerId} has no peer logic initialized.");
            }

            return (IPeerLogicBase)peerLogicBase;
        }
    }
}