using Game.Application.Objects;
using InterestManagement;

namespace Game.Application.Components
{
    public class GameScene : IGameScene
    {
        public PlayerSpawnData PlayerSpawnData { get; set; }

        private IScene<IGameObject> scene;

        public GameScene(IScene<IGameObject> scene)
        {
            this.scene = scene;
        }

        public IScene<IGameObject> GetScene()
        {
            return scene;
        }

        public void Dispose()
        {
            scene?.Dispose();
        }
    }
}