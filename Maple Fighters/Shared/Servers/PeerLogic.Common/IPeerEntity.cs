using ComponentModel.Common;

namespace PeerLogic.Common
{
    public interface IPeerEntity : IEntity
    {
        int Id { get; }
        IContainer<IPeerEntity> Container { get; }
    }
}