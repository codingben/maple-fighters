using Game.Application.Objects;
using InterestManagement;
using Physics.Box2D;

namespace Game.Application.Components
{
    public interface IGameScene : IScene<IGameObject>
    {
        IGameObjectCollection GameObjectCollection { get; }

        IPhysicsExecutor PhysicsExecutor { get; }

        IPhysicsWorldManager PhysicsWorldManager { get; }
    }
}