using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;

namespace Game.Application.Components
{
    public class TheDarkForestGameScene : GameScene
    {
        private readonly IIdGenerator idGenerator;

        public TheDarkForestGameScene(IIdGenerator idGenerator)
            : base(new Vector2(30, 30), new Vector2(10, 5))
        {
            this.idGenerator = idGenerator;

            // TODO: Use another way to add game objects
            var gameObjects = new IGameObject[]
            {
                CreatePortal(),
                CreateBlueSnail()
            };

            Components.Add(new PlayerSpawnDataProvider(GetPlayerSpawnData()));
            Components.Add(new GameObjectCollection(gameObjects));
        }

        PlayerSpawnData GetPlayerSpawnData()
        {
            // TODO: Refactor this method

            return new PlayerSpawnData(new Vector2(-12.8f, -2.95f), new Vector2(10, 5));
        }

        IGameObject CreatePortal()
        {
            // TODO: Refactor this method

            var id = idGenerator.GenerateId();
            var position = new Vector2(12.5f, -1.125f);
            var scene = this;
            var map = (byte)Map.Lobby;

            return new PortalGameObject(
                id,
                position,
                scene,
                map);
        }

        IGameObject CreateBlueSnail()
        {
            // TODO: Refactor this method

            var id = idGenerator.GenerateId();
            var position = new Vector2(-2f, -8.2f);
            var scene = this;

            return new BlueSnailGameObject(
                id,
                position,
                scene);
        }
    }
}