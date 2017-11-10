using ComponentModel.Common;

namespace Game.InterestManagement
{
    public interface ISceneEntity : IEntity
    {
        IContainer<ISceneEntity> Container { get; }
    }
}