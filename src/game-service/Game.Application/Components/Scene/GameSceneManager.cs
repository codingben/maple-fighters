using System.Collections.Generic;
using System.Net;
using Game.Application.Objects;
using InterestManagement;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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

            var yamlConfig = LoadYamlConfig();
            var configData = ParseConfigData(yamlConfig);

            CreateGameScene(configData);
        }

        protected override void OnRemoved()
        {
            gameSceneCollection.Dispose();
        }

        private void CreateGameScene(SceneCollectionData sceneCollection)
        {
            var scenes = sceneCollection.Scenes;

            foreach (var sceneData in scenes)
            {
                var sceneName = sceneData.Key;
                var scene = sceneData.Value;
                var sceneSize = new Vector2(scene.SceneSize.X, scene.SceneSize.Y);
                var regionSize = new Vector2(scene.RegionSize.X, scene.RegionSize.Y);
                var playerSpawn = scene.PlayerSpawn;
                var playerSpawnPosition = new Vector2(playerSpawn.Position.X, playerSpawn.Position.Y);
                var playerSpawnSize = new Vector2(playerSpawn.Size.X, playerSpawn.Size.Y);
                var playerSpawnDirection = playerSpawn.Direction;
                var objects = scene.Objects;
                var gameScene = new GameScene(sceneSize, regionSize);
                var scenePlayerSpawnData = gameScene.Components.Get<IScenePlayerSpawnData>();

                scenePlayerSpawnData.SetPosition(playerSpawnPosition);
                scenePlayerSpawnData.SetSize(playerSpawnSize);
                scenePlayerSpawnData.SetDirection(playerSpawnDirection);

                gameSceneCollection.Add(sceneName, gameScene);

                AddGameObjects(gameScene, objects);
            }
        }

        private void AddGameObjects(IGameScene gameScene, ObjectData[] objects)
        {
            var gameObjects = CreateGameObjects(objects);

            foreach (var gameObject in gameObjects)
            {
                var presenceSceneProvider = gameObject.Components.Get<IPresenceSceneProvider>();
                presenceSceneProvider.SetScene(gameScene);

                var sceneObjectCollection = gameScene.Components.Get<ISceneObjectCollection>();
                sceneObjectCollection.Add(gameObject);
            }
        }

        private IEnumerable<IGameObject> CreateGameObjects(ObjectData[] objects)
        {
            foreach (var objectData in objects)
            {
                var name = objectData.Name;
                var type = (ObjectTypes)objectData.Type;
                var position = new Vector2(objectData.Position.X, objectData.Position.Y);
                var size = new Vector2(objectData.Size.X, objectData.Size.Y);
                var customData = objectData.CustomData;
                var id = idGenerator.GenerateId();
                var gameObject = CreateGameObject(type, id, name, customData);

                gameObject.Transform.SetPosition(position);
                gameObject.Transform.SetSize(size);

                yield return gameObject;
            }
        }

        private IGameObject CreateGameObject(
          ObjectTypes type,
          int id,
          string name,
          string customData)
        {
            if (type == ObjectTypes.Npc) return new NpcGameObject(id, name);
            else if (type == ObjectTypes.Portal) return new PortalGameObject(id, name, customData);
            else if (type == ObjectTypes.Mob) return new MobGameObject(id, name);
            else throw new InvalidGameObjectTypeException(type);
        }

        private SceneCollectionData ParseConfigData(string data)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<SceneCollectionData>(data);
        }

        private string LoadYamlConfig()
        {
            var config = string.Empty;
            var url = "https://raw.githubusercontent.com/codingben/maple-fighters-configs/main/{0}";
            var yamlPath = string.Format(url, "scenes.yml");

            using (var client = new WebClient())
            {
                config = client.DownloadString(yamlPath);
            }

            return config;
        }
    }
}