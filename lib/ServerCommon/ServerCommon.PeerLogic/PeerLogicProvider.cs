using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic
{
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

        public void SetPeerLogic<TPeerLogic>(TPeerLogic peerLogic)
            where TPeerLogic : IPeerLogicBase
        {
            peer.Fiber.Enqueue(() =>
            {
                peer.NetworkTrafficState = NetworkTrafficState.Paused;

                RemovePeerLogic();

                peerLogicBase = peerLogic as PeerLogicBase<TPeer>;
                peerLogicBase?.Initialize(peer, peerId);

                peer.NetworkTrafficState = NetworkTrafficState.Flowing;
            });
        }

        public void RemovePeerLogic()
        {
            peerLogicBase?.Dispose();
        }

        public IPeerLogicBase ProvidePeerLogic()
        {
            if (peerLogicBase == null)
            {
                throw new PeerLogicException(
                    $"A peer with id {peerId} has no peer logic initialized.");
            }

            return peerLogicBase;
        }
    }
}