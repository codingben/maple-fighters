using Common.MathematicsHelper;
using Game.Application.Objects;
using InterestManagement;

namespace Game.Application.Components
{
    public class GameScene : Scene<IGameObject>, IGameScene
    {
        public PlayerSpawnData PlayerSpawnData { get; set; }

        public GameScene(Vector2 worldSize, Vector2 regionSize)
            : base(worldSize, regionSize)
        {
            // Left blank intentionally
        }
    }
}