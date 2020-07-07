using Game.Application.Objects;
using InterestManagement;

namespace Game.Application.Components
{
    public interface IGameScene : IScene<IGameObject>
    {
        IGameObjectCollection GameObjectCollection { get; }

        IGameScenePhysicsExecutor GameScenePhysicsExecutor { get; }
    }
}