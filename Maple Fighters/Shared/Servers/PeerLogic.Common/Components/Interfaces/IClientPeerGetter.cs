using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components.Interfaces
{
    public interface IClientPeerGetter
    {
        int PeerId { get; }
        IClientPeer Peer { get; }
    }
}