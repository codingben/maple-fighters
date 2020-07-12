using Game.Application.Objects;
using InterestManagement;
using Physics.Box2D;

namespace Game.Application.Components
{
    public interface IGameScene
    {
        IMatrixRegion<IGameObject> MatrixRegion { get; }

        IGameObjectCollection GameObjectCollection { get; }

        IGamePlayerSpawnData GamePlayerSpawnData { get; }

        IPhysicsExecutor PhysicsExecutor { get; }

        IPhysicsWorldManager PhysicsWorldManager { get; }
    }
}