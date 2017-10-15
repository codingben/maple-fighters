using System.Collections.Generic;
using CommonTools.Log;
using Scripts.Gameplay;
using Scripts.Gameplay.Actors;
using Scripts.Utils;
using Shared.Game.Common;
using UnityEngine;
using GameObject = UnityEngine.GameObject;
using Object = UnityEngine.Object;

namespace Scripts.Containers
{
    public class GameObjectsContainer : DontDestroyOnLoad<GameObjectsContainer>
    {
        private const string GAME_OBJECTS_FOLDER_PATH = "Game/{0}";
        private int localGameObjectId;
        private readonly Dictionary<int, IGameObject> gameObjects = new Dictionary<int, IGameObject>();

        private void Start()
        {
            SubscribeToGameServiceEvents();

            ServiceContainer.GameService.EnterWorld();
        }

        private void OnDestroy()
        {
            UnsubscribeFromGameServiceEvents();
        }

        private void SubscribeToGameServiceEvents()
        {
            ServiceContainer.GameService.LocalGameObjectAdded.AddListener(CreateLocalGameObject);
            ServiceContainer.GameService.GameObjectAdded.AddListener(OnGameObjectAdded);
            ServiceContainer.GameService.GameObjectRemoved.AddListener(OnGameObjectRemoved);
            ServiceContainer.GameService.GameObjectsAdded.AddListener(OnGameObjectsAdded);
            ServiceContainer.GameService.GameObjectsRemoved.AddListener(OnGameObjectsRemoved);
        }

        private void UnsubscribeFromGameServiceEvents()
        {
            ServiceContainer.GameService.LocalGameObjectAdded.RemoveListener(CreateLocalGameObject);
            ServiceContainer.GameService.GameObjectAdded.RemoveListener(OnGameObjectAdded);
            ServiceContainer.GameService.GameObjectRemoved.RemoveListener(OnGameObjectRemoved);
            ServiceContainer.GameService.GameObjectsAdded.RemoveListener(OnGameObjectsAdded);
            ServiceContainer.GameService.GameObjectsRemoved.RemoveListener(OnGameObjectsRemoved);
        }

        private void CreateLocalGameObject(LocalGameObjectAddedEventParameters parameters)
        {
            var characterGameObject = parameters.CharacterGameObject;
            var character = parameters.Character;

            LogUtils.Log(MessageBuilder.Trace($"Local Game Object Id: {characterGameObject.Id}"));

            var obj = AddGameObject(characterGameObject);
            if (obj == null)
            {
                return;
            }

            obj.GetComponent<CharacterCreator>().Create(new CharacterInformation(characterGameObject.Id, character.Name, character.CharacterType));

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

            var hasCharacter = parameters.HasCharacter;
            if (hasCharacter)
            {
                var character = parameters.CharacterInformation;
                obj.GetComponent<CharacterCreator>().Create(character);
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
                AddGameObject(gameObject);
            }

            foreach (var character in parameters.CharacterInformations)
            {
                var gameObject = GetRemoteGameObject(character.GameObjectId);
                gameObject?.GetGameObject().GetComponent<CharacterCreator>().Create(character);
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

            var networkIdentity = obj.GetComponent<IGameObject>();
            networkIdentity.Id = gameObject.Id;

            gameObjects.Add(gameObject.Id, networkIdentity);

            LogUtils.Log(MessageBuilder.Trace($"Added a new game object with id #{gameObject.Id}"));
            return obj;
        }

        private void RemoveGameObject(int id)
        {
            var gameObject = GetRemoteGameObject(id)?.GetGameObject();
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

        public IGameObject GetRemoteGameObject(int id)
        {
            IGameObject gameObject;
            if (gameObjects.TryGetValue(id, out gameObject))
            {
                return gameObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a game object with id #{id}"), LogMessageType.Warning);
            return null;
        }
    }
}