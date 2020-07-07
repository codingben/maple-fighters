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

        public IGameScenePhysicsExecutor GameScenePhysicsExecutor { get; }

        public GameScene(Vector2 sceneSize, Vector2 regionSize)
        {
            MatrixRegion = new MatrixRegion<IGameObject>(sceneSize, regionSize);
            GameObjectCollection = new GameObjectCollection();
            GameScenePhysicsExecutor = new GameScenePhysicsExecutor(CreateWorldManager());
        }

        public void Dispose()
        {
            MatrixRegion?.Dispose();
            GameScenePhysicsExecutor?.Dispose();
        }

        private IWorldManager CreateWorldManager()
        {
            var lowerBound = new Vector2(-100, -100);
            var upperBound = new Vector2(100, 100);
            var gravity = new Vector2(0, -9.8f);
            var doSleep = false;
            var continuousPhysics = false;

            return new WorldManager(lowerBound, upperBound, gravity, doSleep, continuousPhysics);
        }
    }
}