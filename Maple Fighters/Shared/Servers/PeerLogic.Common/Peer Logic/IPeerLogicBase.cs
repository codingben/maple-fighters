using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common
{
    public interface IPeerLogicBase : IEntity
    {
        void Initialize(IClientPeerWrapper<IClientPeer> peer);
    }
}