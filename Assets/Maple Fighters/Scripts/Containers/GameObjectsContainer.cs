using System.Collections.Generic;
using CommonTools.Log;
using Scripts.Gameplay;
using Scripts.Gameplay.Actors;
using Shared.Game.Common;
using UnityEngine;
using GameObject = UnityEngine.GameObject;
using Object = UnityEngine.Object;

namespace Scripts.Containers
{
    public class GameObjectsContainer : IGameObjectsContainer
    {
        private const string GAME_OBJECTS_FOLDER_PATH = "Game/{0}";
        private int localGameObjectId;
        private readonly Dictionary<int, IGameObject> gameObjects = new Dictionary<int, IGameObject>();

        public GameObjectsContainer()
        {
            ServiceContainer.GameService.GameObjectAdded.AddListener(OnGameObjectAdded);
            ServiceContainer.GameService.GameObjectRemoved.AddListener(OnGameObjectRemoved);
            ServiceContainer.GameService.GameObjectsAdded.AddListener(OnGameObjectsAdded);
            ServiceContainer.GameService.GameObjectsRemoved.AddListener(OnGameObjectsRemoved);
        }

        public void CreateLocalGameObject(Shared.Game.Common.GameObject characterGameObject, Character character)
        {
            LogUtils.Log(MessageBuilder.Trace($"Local Game Object Id: {characterGameObject.Id}"));

            var obj = AddGameObject(characterGameObject);
            if (obj == null)
            {
                return;
            }

            obj.GetComponent<CharacterCreator>().Create(new CharacterInformation(character.Name, character.CharacterType));

            localGameObjectId = characterGameObject.Id;
        }

        private void OnGameObjectAdded(GameObjectAddedEventParameters parameters)
        {
            var gameObject = parameters.GameObject;
            var obj = AddGameObject(gameObject);
            if (obj == null)
            {
                return;
            }

            var character = parameters.CharacterInformation;
            if (character.HasValue)
            {
                obj.GetComponent<CharacterCreator>().Create(character.Value);
            }
        }

        private void OnGameObjectRemoved(GameObjectRemovedEventParameters parameters)
        {
            var id = parameters.GameObjectId;
            RemoveGameObject(id);
        }

        private void OnGameObjectsAdded(GameObjectsAddedEventParameters parameters) // TODO: Bug spotted - characters information may be null or may not exists at all.
        {
            var gameObjects = parameters.GameObjects;
            foreach (var gameObject in gameObjects)
            {
                var obj = AddGameObject(gameObject);
                if (obj == null)
                {
                    continue;
                }

                foreach (var character in parameters.CharacterInformations)
                {
                    obj.GetComponent<CharacterCreator>().Create(character);
                }
            }
        }

        private void OnGameObjectsRemoved(GameObjectsRemovedEventParameters parameters)
        {
            var ids = parameters.GameObjectsId;
            foreach (var id in ids)
            {
                RemoveGameObject(id);
            }
        }

        private GameObject AddGameObject(Shared.Game.Common.GameObject gameObject)
        {
            if (gameObjects.ContainsKey(gameObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"Game object with id #{gameObject.Id} already exists."), LogMessageType.Warning);
                return null;
            }

            var position = new Vector3(gameObject.X, gameObject.Y);
            var obj = CreateGameObject(gameObject.Name, position);
            if (obj == null)
            {
                return null;
            }

            obj.name = gameObject.Name;

            LogUtils.Log(MessageBuilder.Trace($"Created object: {obj.gameObject.name}"));

            var networkIdentity = obj.GetComponent<IGameObject>();
            networkIdentity.Id = gameObject.Id;

            gameObjects.Add(gameObject.Id, networkIdentity);

            LogUtils.Log(MessageBuilder.Trace($"Added a new game object with id #{gameObject.Id}"));
            return obj;
        }

        private void RemoveGameObject(int id)
        {
            var gameObject = GetRemoteGameObject(id);
            if (gameObject == null)
            {
                return;
            }

            gameObjects.Remove(id);

            Object.Destroy(gameObject);

            LogUtils.Log(MessageBuilder.Trace($"Removed a game object with id #{id}"));
        }

        private GameObject CreateGameObject(string name, Vector3 position)
        {
            var obj = Resources.Load(string.Format(GAME_OBJECTS_FOLDER_PATH, name)).AssertNotNull();
            if (obj != null)
            {
                return Object.Instantiate(obj, position, Quaternion.identity) as GameObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a game object with name {name}"), LogMessageType.Error);
            return null;
        }

        public IGameObject GetLocalGameObject()
        {
            IGameObject gameObject;
            if (gameObjects.TryGetValue(localGameObjectId, out gameObject))
            {
                return gameObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a local game object with id #{localGameObjectId}"), LogMessageType.Warning);
            return null;
        }

        public GameObject GetRemoteGameObject(int id)
        {
            IGameObject gameObject;
            if (gameObjects.TryGetValue(id, out gameObject))
            {
                return gameObject.GetGameObject();
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a game object with id #{id}"), LogMessageType.Warning);
            return null;
        }
    }
}