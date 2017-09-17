using ServerApplication.Common.ComponentModel;
using Shared.Game.Common;

namespace Game.InterestManagement
{
    public interface IGameObject : IEntity
    {
        int Id { get; }
        IScene Scene { get; }
        IContainer<IGameObject> Container { get; }

        void ChangeScene(Maps map);
    }
}