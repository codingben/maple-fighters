using System;
using Game.Application.Objects;
using InterestManagement;
using Game.Physics;

namespace Game.Application.Components
{
    public interface IGameScene : IDisposable
    {
        IMatrixRegion<IGameObject> MatrixRegion { get; }

        IGameObjectCollection GameObjectCollection { get; }

        ISpawnData SpawnData { get; }

        IPhysicsExecutor PhysicsExecutor { get; }

        IPhysicsWorldManager PhysicsWorldManager { get; }
    }
}