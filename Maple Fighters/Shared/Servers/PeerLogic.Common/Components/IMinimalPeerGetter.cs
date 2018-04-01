using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    public interface IMinimalPeerGetter
    {
        int PeerId { get; }
        IMinimalPeer Peer { get; }
    }
}