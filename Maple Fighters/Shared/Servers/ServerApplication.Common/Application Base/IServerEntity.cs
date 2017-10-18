using ComponentModel.Common;

namespace ServerApplication.Common.ApplicationBase
{
    public interface IServerEntity : IEntity
    {
        IContainer<IServerEntity> Container { get; }
    }
}