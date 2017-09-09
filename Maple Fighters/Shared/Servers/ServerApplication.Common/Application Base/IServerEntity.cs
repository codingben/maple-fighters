using ServerApplication.Common.ComponentModel;

namespace ServerApplication.Common.ApplicationBase
{
    public interface IServerEntity : IEntity
    {
        IContainer<IServerEntity> Container { get; }
    }
}