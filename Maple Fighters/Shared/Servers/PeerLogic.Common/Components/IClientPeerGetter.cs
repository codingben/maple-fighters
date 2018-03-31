using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    public interface IClientPeerGetter : IExposableComponent
    {
        int PeerId { get; }
        IClientPeer Peer { get; }
    }
}