using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    public interface IClientPeerGetter
    {
        int PeerId { get; }
        IClientPeer Peer { get; }
    }
}