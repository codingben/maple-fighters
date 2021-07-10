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
            gameSceneCollection.Add(Map.Lobby, CreateLobby());
            gameSceneCollection.Add(Map.TheDarkForest, CreateTheDarkForest());
        }

        protected override void OnRemoved()
        {
            var maps = new[]
            {
                Map.Lobby,
                Map.TheDarkForest
            };

            foreach (var map in maps)
            {
                if (gameSceneCollection.TryGet(map, out var gameScene))
                {
                    gameScene?.Dispose();
                }

                gameSceneCollection.Remove(map);
            }
        }

        private IGameScene CreateLobby()
        {
            var sceneSize = new Vector2(40, 5);
            var regionSize = new Vector2(10, 5);
            var gameScene = new GameScene(sceneSize, regionSize);
            gameScene.SpawnData.Position = new Vector2(18, -1.86f);
            gameScene.SpawnData.Size = new Vector2(10, 5); // NOTE: Size should be the same as region size
            gameScene.SpawnData.Direction = 1;

            foreach (var gameObject in CreateLobbyGameObjects(gameScene))
            {
                gameScene.GameObjectCollection.Add(gameObject);
            }

            return gameScene;
        }

        private IEnumerable<IGameObject> CreateLobbyGameObjects(IGameScene gameScene)
        {
            // Guardian Game Object #1
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(-14.24f, -2.025f);
                var size = new Vector2(10, 5); // NOTE: Size should be the same as region size
                var guardian = new GuardianGameObject(id, position, size, new IComponent[]
                {
                    new PresenceMapProvider(gameScene),
                    new GuardianIdleBehaviour(text: "Hello! Enter <color=red>T</color> to teleport.", time: 5)
                });

                yield return guardian;
            }

            // Portal Game Object #2
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(-17.125f, -1.5f);
                var size = new Vector2(10, 5); // NOTE: Size should be the same as region size
                var portal = new PortalGameObject(id, position, size, new IComponent[]
                {
                    new PresenceMapProvider(gameScene),
                    new PortalData(map: (byte)Map.TheDarkForest)
                });

                yield return portal;
            }
        }

        private IGameScene CreateTheDarkForest()
        {
            var sceneSize = new Vector2(30, 30);
            var regionSize = new Vector2(10, 5);
            var gameScene = new GameScene(sceneSize, regionSize);
            gameScene.SpawnData.Position = new Vector2(-12.8f, -12.95f);
            gameScene.SpawnData.Size = new Vector2(10, 5); // NOTE: Size should be the same as region size
            gameScene.SpawnData.Direction = -1;

            foreach (var gameObject in CreateTheDarkForestGameObjects(gameScene))
            {
                gameScene.GameObjectCollection.Add(gameObject);
            }

            return gameScene;
        }

        private IEnumerable<IGameObject> CreateTheDarkForestGameObjects(IGameScene gameScene)
        {
            // Blue Snail Game Object #1
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(-6f, -8.2f);
                var size = new Vector2(10, 5); // NOTE: Size should be the same as region size
                var coroutineRunner = gameScene.PhysicsExecutor.GetCoroutineRunner();
                var blueSnail = new BlueSnailGameObject(id, position, size, new IComponent[]
                {
                    new PresenceMapProvider(gameScene),
                    new BlueSnailMoveBehaviour(coroutineRunner),
                    new PhysicsBodyPositionSetter()
                });

                var bodyData = blueSnail.CreateBodyData();
                gameScene.PhysicsWorldManager.AddBody(bodyData);

                yield return blueSnail;
            }

            // Portal Game Object #2
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(12.5f, -1.125f);
                var size = new Vector2(10, 5); // NOTE: Size should be the same as region size
                var portal = new PortalGameObject(id, position, size, new IComponent[]
                {
                    new PresenceMapProvider(gameScene),
                    new PortalData(map: (byte)Map.Lobby)
                });

                yield return portal;
            }
        }
    }
}
