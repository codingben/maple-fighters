using ComponentModel.Common;

namespace PeerLogic.Common
{
    public interface IPeerLogicBase : IEntity
    {
        void Initialize(IClientPeerWrapper peer);
    }
}