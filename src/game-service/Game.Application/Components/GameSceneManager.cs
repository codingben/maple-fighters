using System.Collections.Generic;
using Common.ComponentModel;
using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Components
{
    public class GameSceneManager : ComponentBase
    {
        private IIdGenerator idGenerator;
        private IGameSceneCollection gameSceneCollection;

        protected override void OnAwake()
        {
            idGenerator = Components.Get<IIdGenerator>();
            gameSceneCollection = Components.Get<IGameSceneCollection>();

            var lobby = CreateLobby();
            var theDarkForest = CreateTheDarkForest();

            gameSceneCollection.Add(Map.Lobby, lobby);
            gameSceneCollection.Add(Map.TheDarkForest, theDarkForest);
        }

        protected override void OnRemoved()
        {
            gameSceneCollection.Remove(Map.Lobby);
            gameSceneCollection.Remove(Map.TheDarkForest);
        }

        // TODO: Refactor
        private IGameScene CreateLobby()
        {
            var gameScene = new GameScene(new Vector2(40, 5), new Vector2(10, 5));

            // Lobby Spawn Position
            gameScene.GamePlayerSpawnData.SetSpawnPosition(new Vector2(18, -1.86f));

            foreach (var gameObject in CreateLobbyGameObjects(gameScene))
            {
                gameScene.GameObjectCollection.Add(gameObject);
            }

            return gameScene;
        }

        // TODO: Refactor
        private IEnumerable<IGameObject> CreateLobbyGameObjects(IGameScene gameScene)
        {
            // Guardian Game Object #1
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(-14.24f, -2.025f);
                var guardian = new GuardianGameObject(id, position);
                var presenceMapProvider = guardian.Components.Get<IPresenceMapProvider>();
                presenceMapProvider.SetMap(gameScene);

                guardian.AddBubbleNotification("Hello", 1);

                yield return guardian;
            }

            // Portal Game Object #2
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(-17.125f, -1.5f);
                var portal = new PortalGameObject(id, position);
                var presenceMapProvider = portal.Components.Get<IPresenceMapProvider>();
                presenceMapProvider.SetMap(gameScene);

                portal.AddPortalData((byte)Map.TheDarkForest);

                yield return portal;
            }
        }

        // TODO: Refactor
        private IGameScene CreateTheDarkForest()
        {
            var gameScene = new GameScene(new Vector2(30, 30), new Vector2(10, 5));

            // The Dark Forest Spawn Position
            gameScene.GamePlayerSpawnData.SetSpawnPosition(new Vector2(-12.8f, -2.95f));

            foreach (var gameObject in CreateTheDarkForestGameObjects(gameScene))
            {
                gameScene.GameObjectCollection.Add(gameObject);
            }

            return gameScene;
        }

        // TODO: Refactor
        private IEnumerable<IGameObject> CreateTheDarkForestGameObjects(IGameScene gameScene)
        {
            // Blue Snail Game Object #1
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(-2f, -8.2f);
                var components = new IComponent[]
                {
                    new BlueSnailMoveBehaviour()
                };
                var blueSnail = new BlueSnailGameObject(id, position, components);
                var presenceMapProvider = blueSnail.Components.Get<IPresenceMapProvider>();
                presenceMapProvider.SetMap(gameScene);

                var bodyData = blueSnail.CreateBodyData();
                gameScene.PhysicsWorldManager.AddBody(bodyData);

                yield return blueSnail;
            }

            // Portal Game Object #2
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(12.5f, -1.125f);
                var portal = new PortalGameObject(id, position);
                var presenceMapProvider = portal.Components.Get<IPresenceMapProvider>();
                presenceMapProvider.SetMap(gameScene);

                portal.AddPortalData((byte)Map.Lobby);

                yield return portal;
            }
        }
    }
}