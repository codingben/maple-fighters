using System.Collections.Generic;
using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;

namespace Game.Application.Components
{
    public class LobbyGameScene : GameScene
    {
        private readonly IIdGenerator idGenerator;
        private readonly IList<IGameObject> gameObjects;

        public LobbyGameScene(IIdGenerator idGenerator)
            : base(new Vector2(40, 5), new Vector2(10, 5))
        {
            this.idGenerator = idGenerator;

            // TODO: Who will "destroy" game objects?

            gameObjects = new List<IGameObject>()
            {
                CreateGuardian(),
                CreatePortal()
            };

            Components.Add(new PlayerSpawnDataProvider(GetPlayerSpawnData()));
        }

        PlayerSpawnData GetPlayerSpawnData()
        {
            return new PlayerSpawnData(new Vector2(18, -1.86f), new Vector2(10, 5));
        }

        IGameObject CreateGuardian()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-14.24f, -2.025f);
            var scene = this;
            var text = "Hello!";
            var time = 5;

            return new GuardianGameObject(
                id,
                position,
                scene,
                text,
                time);
        }

        IGameObject CreatePortal()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-17.125f, -1.5f);
            var scene = this;
            var map = (byte)Map.TheDarkForest;

            return new PortalGameObject(
                id,
                position,
                scene,
                map);
        }
    }
}