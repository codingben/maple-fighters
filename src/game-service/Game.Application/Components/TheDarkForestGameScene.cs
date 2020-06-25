using System.Collections.Generic;
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

            Components.Add(new PlayerSpawnDataProvider(GetPlayerSpawnData()));
            Components.Add(new GameObjectCollection(GetGameObjects()));
        }

        PlayerSpawnData GetPlayerSpawnData()
        {
            return new PlayerSpawnData(new Vector2(-12.8f, -2.95f), new Vector2(10, 5));
        }

        IEnumerable<IGameObject> GetGameObjects()
        {
            yield return CreatePortal();
            yield return CreateBlueSnail();
        }

        IGameObject CreatePortal()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(12.5f, -1.125f);
            var portal = new PortalGameObject(id, this);

            portal.AddPortalData((byte)Map.Lobby);

            return portal;
        }

        IGameObject CreateBlueSnail()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-2f, -8.2f);

            return new BlueSnailGameObject(id, this);
        }
    }
}