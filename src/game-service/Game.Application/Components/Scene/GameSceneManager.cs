using System.Collections.Generic;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using InterestManagement;

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
            gameScene.PlayerSpawnData.SetPosition(new Vector2(18, -1.86f));
            gameScene.PlayerSpawnData.SetSize(new Vector2(10, 5));
            gameScene.PlayerSpawnData.SetDirection(1);

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
                var npc = new NpcGameObject(id, name: "Guardian");
                npc.Transform.SetPosition(new Vector2(-14.24f, -2.025f));
                npc.Transform.SetSize(new Vector2(10, 5));

                var presenceMapProvider = npc.Components.Get<IPresenceMapProvider>();
                presenceMapProvider.SetMap(gameScene);

                yield return npc;
            }

            // Portal Game Object #2
            {
                var id = idGenerator.GenerateId();
                var portal = new PortalGameObject(id, "Portal");
                portal.Transform.SetPosition(new Vector2(-17.125f, -1.5f));
                portal.Transform.SetSize(new Vector2(10, 5));

                var presenceMapProvider = portal.Components.Get<IPresenceMapProvider>();
                presenceMapProvider.SetMap(gameScene);

                yield return portal;
            }
        }

        private IGameScene CreateTheDarkForest()
        {
            var gameScene = new GameScene(sceneSize: new Vector2(30, 30), regionSize: new Vector2(10, 5));
            gameScene.PlayerSpawnData.SetPosition(new Vector2(-12.8f, -12.95f));
            gameScene.PlayerSpawnData.SetSize(new Vector2(10, 5));
            gameScene.PlayerSpawnData.SetDirection(-1);

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
                var size = new Vector2(10, 5);

                foreach (var position in positions)
                {
                    yield return CreateMob(gameScene, name: "BlueSnail", position, size);
                }
            }

            // Mob Game Objects #2
            {
                var positions = new Vector2[2]
                {
                    new Vector2(-6.5f, 3.75f),
                    new Vector2(-0.8f, 3.75f)
                };
                var size = new Vector2(10, 5);

                foreach (var position in positions)
                {
                    yield return CreateMob(gameScene, name: "Mushroom", position, size);
                }
            }

            // Portal Game Object #3
            {
                var id = idGenerator.GenerateId();
                var portal = new PortalGameObject(id, name: "Portal");
                portal.Transform.SetPosition(new Vector2(12.5f, -1.125f));
                portal.Transform.SetSize(new Vector2(10, 5));

                var presenceMapProvider = portal.Components.Get<IPresenceMapProvider>();
                presenceMapProvider.SetMap(gameScene);

                yield return portal;
            }
        }

        private IGameObject CreateMob(IGameScene gameScene, string name, Vector2 position, Vector2 size)
        {
            var id = idGenerator.GenerateId();
            var mob = new MobGameObject(id, name);
            mob.Transform.SetPosition(position);
            mob.Transform.SetSize(size);

            var presenceMapProvider = mob.Components.Get<IPresenceMapProvider>();
            presenceMapProvider.SetMap(gameScene);

            var coroutineRunner = gameScene.PhysicsExecutor.GetCoroutineRunner();
            var mobMoveBehaviour = mob.Components.Get<IMobMoveBehaviour>();
            mobMoveBehaviour.SetCoroutineRunner(coroutineRunner);
            mobMoveBehaviour.StartMove();

            gameScene.PhysicsWorldManager.AddBody(mob.CreateBodyData());

            return mob;
        }

        private string GetConfig()
        {
            return @"
scenes:
  lobby:
    sceneSize:
      x: 40
      y: 5
    regionSize: &regionSize
      x: 10
      y: 5
    playerSpawn:
      position:
        x: 18
        y: -1.86
      size: *regionSize
      direction: 1
    objects:
    - name: Guardian
      type: 0 # NPC
      position:
        x: -14.24
        y: -2.025
      size: *regionSize
    - name: Portal
      type: 1 # Portal
      position:
        x: -17.125
        y: -1.5
      size: *regionSize
  thedarkforest:
    sceneSize:
      x: 30
      y: 30
    regionSize: &regionSize
      x: 10
      y: 5
    playerSpawn:
      position:
        x: -12.8
        y: -12.95
      size: *regionSize
      direction: -1
    objects:
    - name: BlueSnail
      type: 0 # NPC
      position:
        x: -2.5
        y: -8.15
      size: *regionSize
    - name: BlueSnail
      type: 2 # Mob
      position:
        x: -2.85
        y: -3.05
      size: *regionSize
    - name: BlueSnail
      type: 2 # Mob
      position:
        x: -3.5
        y: -3.05
      size: *regionSize
    - name: Mushroom
      type: 2 # Mob
      position:
        x: -6.5
        y: -3.75
      size: *regionSize
    - name: Mushroom
      type: 2 # Mob
      position:
        x: -0.8
        y: -3.75
      size: *regionSize
    - name: Portal
      type: 1 # Portal
      position:
        x: -12.5
        y: -1.125
      size: *regionSize
            ";
        }
    }
}