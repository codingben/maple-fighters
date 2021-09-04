using Common.MathematicsHelper;
using Game.Application.Objects;
using InterestManagement;
using Game.Physics;

namespace Game.Application.Components
{
    public class GameScene : IGameScene
    {
        public IMatrixRegion<IGameObject> MatrixRegion { get; }

        public IGameObjectCollection GameObjectCollection { get; }

        public ISpawnData SpawnData { get; }

        public IPhysicsExecutor PhysicsExecutor { get; }

        public IPhysicsWorldManager PhysicsWorldManager { get; }

        public GameScene(Vector2 sceneSize, Vector2 regionSize)
        {
            var log = InterestManagementLogger.GetLogger();

            MatrixRegion = new MatrixRegion<IGameObject>(sceneSize, regionSize, log);
            GameObjectCollection = new GameObjectCollection();
            SpawnData = new PlayerSpawnData();

            var lowerBound = new Vector2(-100, -100);
            var upperBound = new Vector2(100, 100);
            var gravity = new Vector2(0, 0 /* -9.81f */);

            PhysicsWorldManager = new PhysicsWorldManager(lowerBound, upperBound, gravity);
            PhysicsExecutor = new PhysicsExecutor(OnUpdateBodies, OnSimulatePhysics);
        }

        public void Dispose()
        {
            MatrixRegion?.Dispose();
            PhysicsExecutor?.Dispose();
            PhysicsWorldManager?.Dispose();
        }

        private void OnUpdateBodies()
        {
            PhysicsWorldManager?.UpdateBodies();
        }

        private void OnSimulatePhysics()
        {
            var timeStep = PhysicsSettings.TimeStep;
            var velocityIterations = PhysicsSettings.VelocityIterations;
            var positionIterations = PhysicsSettings.PositionIterations;

            PhysicsWorldManager?.Step(timeStep, velocityIterations, positionIterations);
        }
    }
}