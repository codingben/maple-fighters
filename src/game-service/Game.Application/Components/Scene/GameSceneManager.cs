using System.Collections.Generic;
using Box2DX.Dynamics;
using Common.MathematicsHelper;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using Game.Physics;

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
            var gameScene = new GameScene(sceneSize: new Vector2(40, 5), regionSize: new Vector2(10, 5));
            gameScene.SpawnData.Position = new Vector2(18, -1.86f);
            gameScene.SpawnData.Size = new Vector2(10, 5); // TODO: Size should be the same as region size
            gameScene.SpawnData.Direction = 1;

            foreach (var gameObject in CreateLobbyGameObjects(gameScene))
            {
                gameScene.GameObjectCollection.Add(gameObject);
            }

            return gameScene;
        }

        private IEnumerable<IGameObject> CreateLobbyGameObjects(IGameScene gameScene)
        {
            // Npc Game Object #1
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(-14.24f, -2.025f);
                var size = new Vector2(10, 5); // TODO: Size should be the same as region size
                var npc = new GameObject(id, "Guardian", position, size, new IComponent[]
                {
                    new PresenceMapProvider(gameScene),
                    new NpcIdleBehaviour(text: "Hello World!", time: 5)
                });

                yield return npc;
            }

            // Portal Game Object #2
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(-17.125f, -1.5f);
                var size = new Vector2(10, 5); // TODO: Size should be the same as region size
                var portal = new GameObject(id, "Portal", position, size, new IComponent[]
                {
                    new PresenceMapProvider(gameScene),
                    new PortalData(map: (byte)Map.TheDarkForest)
                });

                yield return portal;
            }
        }

        private IGameScene CreateTheDarkForest()
        {
            var gameScene = new GameScene(sceneSize: new Vector2(30, 30), regionSize: new Vector2(10, 5));
            gameScene.SpawnData.Position = new Vector2(-12.8f, -12.95f);
            gameScene.SpawnData.Size = new Vector2(10, 5); // TODO: Size should be the same as region size
            gameScene.SpawnData.Direction = -1;

            foreach (var gameObject in CreateTheDarkForestGameObjects(gameScene))
            {
                gameScene.GameObjectCollection.Add(gameObject);
            }

            return gameScene;
        }

        private IEnumerable<IGameObject> CreateTheDarkForestGameObjects(IGameScene gameScene)
        {
            // Mob Game Objects #1
            {
                var positions = new Vector2[3]
                {
                    new Vector2(-2.5f, -8.15f),
                    new Vector2(2.85f, -3.05f),
                    new Vector2(-3.5f, -3.05f)
                };

                foreach (var position in positions)
                {
                    yield return CreateMob(gameScene, name: "BlueSnail", position);
                }
            }

            // Mob Game Objects #2
            {
                var positions = new Vector2[2]
                {
                    new Vector2(-6.5f, 3.75f),
                    new Vector2(-0.8f, 3.75f)
                };

                foreach (var position in positions)
                {
                    yield return CreateMob(gameScene, name: "Mushroom", position);
                }
            }

            // Portal Game Object #3
            {
                var id = idGenerator.GenerateId();
                var position = new Vector2(12.5f, -1.125f);
                var size = new Vector2(10, 5); // TODO: Size should be the same as region size
                var portal = new GameObject(id, "Portal", position, size, new IComponent[]
                {
                    new PresenceMapProvider(gameScene),
                    new PortalData(map: (byte)Map.Lobby)
                });

                yield return portal;
            }
        }

        private IGameObject CreateMob(IGameScene scene, string name, Vector2 position)
        {
            var id = idGenerator.GenerateId();
            var coroutineRunner = scene.PhysicsExecutor.GetCoroutineRunner();
            var size = new Vector2(10, 5); // TODO: Size should be the same as region size
            var mob = new GameObject(id, name, position, size, new IComponent[]
            {
                new PresenceMapProvider(scene),
                new MobMoveBehaviour(coroutineRunner),
                new PhysicsBodyPositionSetter()
            });

            var bodyDef = new BodyDef();
            var x = mob.Transform.Position.X;
            var y = mob.Transform.Position.Y;
            bodyDef.Position.Set(x, y);

            var polygonDef = new PolygonDef();
            polygonDef.SetAsBox(0.3625f, 0.825f); // TODO: No hard coding
            polygonDef.Density = 0.0f; // TODO: No hard coding
            polygonDef.Filter = new FilterData()
            {
                GroupIndex = (short)LayerMask.Mob
            };
            polygonDef.UserData = new MobContactEvents(id);

            var bodyData = new NewBodyData(id, bodyDef, polygonDef);
            scene.PhysicsWorldManager.AddBody(bodyData);

            return mob;
        }
    }
}