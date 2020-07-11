using Common.MathematicsHelper;
using Game.Application.Objects;
using InterestManagement;
using Physics.Box2D;

namespace Game.Application.Components
{
    public class GameScene : IGameScene
    {
        public IMatrixRegion<IGameObject> MatrixRegion { get; }

        public IGameObjectCollection GameObjectCollection { get; }

        public IGamePlayerSpawnData GamePlayerSpawnData { get; }

        public IPhysicsExecutor PhysicsExecutor { get; }

        public IPhysicsWorldManager PhysicsWorldManager { get; }

        public GameScene(Vector2 sceneSize, Vector2 regionSize)
        {
            MatrixRegion = new MatrixRegion<IGameObject>(sceneSize, regionSize);
            GameObjectCollection = new GameObjectCollection();
            GamePlayerSpawnData = new GamePlayerSpawnData();

            var lowerBound = new Vector2(-100, -100);
            var upperBound = new Vector2(100, 100);
            var gravity = new Vector2(0, -9.8f);

            PhysicsWorldManager = new PhysicsWorldManager(lowerBound, upperBound, gravity);
            PhysicsExecutor = new PhysicsExecutor();
        }

        public void Dispose()
        {
            MatrixRegion?.Dispose();
            PhysicsExecutor?.Dispose();
            PhysicsWorldManager?.Dispose();
        }

        private void OnUpdateBodies()
        {
            PhysicsWorldManager.UpdateBodies();
        }

        private void OnSimulatePhysics()
        {
            var timeStep = DefaultSettings.TimeStep;
            var velocityIterations = DefaultSettings.VelocityIterations;
            var positionIterations = DefaultSettings.PositionIterations;

            PhysicsWorldManager.Step(timeStep, velocityIterations, positionIterations);
        }
    }
}