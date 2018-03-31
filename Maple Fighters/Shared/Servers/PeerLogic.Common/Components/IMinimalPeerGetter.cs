using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    public interface IMinimalPeerGetter : IExposableComponent
    {
        int PeerId { get; }
        IMinimalPeer Peer { get; }
    }
}