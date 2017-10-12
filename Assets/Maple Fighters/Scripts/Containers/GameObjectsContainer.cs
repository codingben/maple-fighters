using System;
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
        public event Action GameObjectsAdded;

        private const string GAME_OBJECTS_FOLDER_PATH = "Game/{0}";
        private int localGameObjectId;
        private readonly Dictionary<int, IGameObject> gameObjects = new Dictionary<int, IGameObject>();

        public GameObjectsContainer()
        {
            ServiceContainer.GameService.LocalGameObjectAdded.AddListener(OnLocalGameObjectAdded);
            ServiceContainer.GameService.GameObjectAdded.AddListener(OnGameObjectAdded);
            ServiceContainer.GameService.GameObjectRemoved.AddListener(OnGameObjectRemoved);
            ServiceContainer.GameService.GameObjectsAdded.AddListener(OnGameObjectsAdded);
            ServiceContainer.GameService.GameObjectsRemoved.AddListener(OnGameObjectsRemoved);
        }

        private void OnLocalGameObjectAdded(EnterWorldResponseParameters parameters)
        {
            var gameObject = parameters.PlayerGameObject;
            var character = new CharacterInformation(parameters.Character.Name, parameters.Character.CharacterType);

            var obj = AddGameObject(gameObject);
            obj.GetComponent<CharacterCreator>().Create(character);

            localGameObjectId = gameObject.Id;
        }

        private void OnGameObjectAdded(GameObjectAddedEventParameters parameters)
        {
            var gameObject = parameters.GameObject;
            var obj = AddGameObject(gameObject);

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

        private void OnGameObjectsAdded(GameObjectsAddedEventParameters parameters)
        {
            var gameObjects = parameters.GameObjects;
            foreach (var gameObject in gameObjects)
            {
                var obj = AddGameObject(gameObject);

                foreach (var characterInformation in parameters.CharacterInformations)
                {
                    obj.GetComponent<CharacterCreator>().Create(characterInformation);
                }
            }

            GameObjectsAdded?.Invoke();
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

            var obj = CreateGameObject(gameObject.Name, new Vector3(gameObject.X, gameObject.Y));
            if (obj == null)
            {
                return null;
            }

            gameObjects.Add(gameObject.Id, obj.GetComponent<IGameObject>());

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