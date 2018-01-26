using ServerCommunicationInterfaces;

namespace PeerLogic.Common
{
    public interface IClientPeerWrapper<out T>
        where T : IClientPeer
    {
        int PeerId { get; }
        T Peer { get; }
        IPeerLogicBase PeerLogic { get; }

        void SetPeerLogic<TPeerLogic>(TPeerLogic peerLogic)
            where TPeerLogic : IPeerLogicBase;
    }
}