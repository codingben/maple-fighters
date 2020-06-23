using System.Collections.Generic;
using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;

namespace Game.Application.Components
{
    public class TheDarkForestGameScene : GameScene
    {
        private readonly IIdGenerator idGenerator;
        private readonly IList<IGameObject> gameObjects;

        public TheDarkForestGameScene(IIdGenerator idGenerator)
            : base(new Vector2(30, 30), new Vector2(10, 5))
        {
            this.idGenerator = idGenerator;

            // TODO: Who will "destroy" game objects?

            gameObjects = new List<IGameObject>()
            {
                CreatePortal(),
                CreateBlueSnail()
            };

            Components.Add(new PlayerSpawnDataProvider(GetPlayerSpawnData()));
        }

        PlayerSpawnData GetPlayerSpawnData()
        {
            return new PlayerSpawnData(new Vector2(-12.8f, - -2.95f), new Vector2(10, 5));
        }

        IGameObject CreatePortal()
        {
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