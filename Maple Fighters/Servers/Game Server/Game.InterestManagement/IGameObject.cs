using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    public interface IGameObject : IEntity
    {
        int Id { get; }

        IScene Scene { get; set; }

        IContainer<IGameObject> Container { get; }
    }
}