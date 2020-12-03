using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components.Interfaces
{
    public interface IClientPeerProvider
    {
        int PeerId { get; }
        IClientPeer Peer { get; }
    }
}