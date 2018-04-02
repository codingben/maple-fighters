using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components.Interfaces
{
    public interface IMinimalPeerGetter
    {
        int PeerId { get; }
        IMinimalPeer Peer { get; }
    }
}