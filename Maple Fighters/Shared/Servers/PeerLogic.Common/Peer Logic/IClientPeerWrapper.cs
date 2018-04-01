using ServerCommunicationInterfaces;

namespace PeerLogic.Common
{
    public interface IClientPeerWrapper : IPeerLogicHandler
    {
        int PeerId { get; }
        IClientPeer Peer { get; }
    }
}