using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace Game.Application.PeerLogic.Components
{
    internal interface IMinimalPeerGetter : IExposableComponent
    {
        IMinimalPeer GetPeer();
    }
}