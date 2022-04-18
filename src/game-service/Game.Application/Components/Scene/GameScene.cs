using InterestManagement;

namespace Game.Application.Components
{
    public class GameScene : IGameScene
    {
        public IComponents Components { get; }

        public GameScene(Vector2 sceneSize, Vector2 regionSize)
        {
            Components = new ComponentCollection();
            Components.Add(new SceneRegionCreator(sceneSize, regionSize));
            Components.Add(new SceneObjectCollection());
            Components.Add(new ScenePlayerSpawnData());
            Components.Add(new ScenePhysicsCreator());
        }

        public void Dispose()
        {
            Components?.Dispose();
        }
    }
}