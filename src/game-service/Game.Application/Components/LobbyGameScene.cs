using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;

namespace Game.Application.Components
{
    public class LobbyGameScene : GameScene
    {
        private readonly IIdGenerator idGenerator;

        public LobbyGameScene(IIdGenerator idGenerator)
            : base(new Vector2(40, 5), new Vector2(10, 5))
        {
            this.idGenerator = idGenerator;

            // TODO: Refactor this
            var gameObjects = new IGameObject[]
            {
                CreateGuardian(),
                CreatePortal()
            };

            Components.Add(new PlayerSpawnDataProvider(GetPlayerSpawnData()));
            Components.Add(new GameObjectCollection(gameObjects));
        }

        PlayerSpawnData GetPlayerSpawnData()
        {
            return new PlayerSpawnData(new Vector2(18, -1.86f), new Vector2(10, 5));
        }

        IGameObject CreateGuardian()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-14.24f, -2.025f);
            var guardian = new GuardianGameObject(id, this);

            guardian.Transform.SetPosition(position);
            guardian.Transform.SetSize(Vector2.One);
            guardian.AddBubbleNotification("Hello", 1);

            return guardian;
        }

        IGameObject CreatePortal()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-17.125f, -1.5f);
            var portal = new PortalGameObject(id, this);

            portal.AddPortalData((byte)Map.TheDarkForest);

            return portal;
        }
    }
}