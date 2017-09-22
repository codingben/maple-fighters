using ServerApplication.Common.ComponentModel;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public interface IPeerEntity : IEntity
    {
        int Id { get; }
        IContainer<IPeerEntity> Container { get; }
    }
}