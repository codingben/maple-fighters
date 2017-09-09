using ServerApplication.Common.ComponentModel;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public interface IPeerEntity : IEntity
    {
        IContainer<IPeerEntity> Container { get; }
    }
}