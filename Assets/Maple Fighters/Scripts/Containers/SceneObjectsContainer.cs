using System.Collections.Generic;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Gameplay;
using Scripts.Gameplay.Actors;
using Scripts.Utils;
using Shared.Game.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Containers
{
    public class SceneObjectsContainer : DontDestroyOnLoad<SceneObjectsContainer>
    {
        private readonly Dictionary<int, ISceneObject> sceneObjects = new Dictionary<int, ISceneObject>();
        private int localSceneObjectId;

        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        protected override void OnAwake()
        {
            base.OnAwake();

            coroutinesExecutor.StartTask(CreateLocalGameObject);
        }

        private void Start()
        {
            SubscribeToEvents();
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            sceneObjects.Remove(localSceneObjectId);

            coroutinesExecutor.StartTask(CreateLocalGameObject);
        }

        private void OnDestroy()
        {
            coroutinesExecutor.Dispose();

            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            ServiceContainer.GameService.SceneObjectAdded.AddListener(OnGameObjectAdded);
            ServiceContainer.GameService.SceneObjectRemoved.AddListener(OnGameObjectRemoved);
            ServiceContainer.GameService.SceneObjectsAdded.AddListener(OnGameObjectsAdded);
            ServiceContainer.GameService.SceneObjectsRemoved.AddListener(OnGameObjectsRemoved);
        }

        private void UnsubscribeFromEvents()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            ServiceContainer.GameService.SceneObjectAdded.RemoveListener(OnGameObjectAdded);
            ServiceContainer.GameService.SceneObjectRemoved.RemoveListener(OnGameObjectRemoved);
            ServiceContainer.GameService.SceneObjectsAdded.RemoveListener(OnGameObjectsAdded);
            ServiceContainer.GameService.SceneObjectsRemoved.RemoveListener(OnGameObjectsRemoved);
        }

        private async Task CreateLocalGameObject(IYield yield)
        {
            var parameters = await ServiceContainer.GameService.EnterScene(yield);

            var characterGameObject = parameters.CharacterSceneObject;
            var character = parameters.Character;

            var position = new Vector2(parameters.CharacterSceneObject.X, parameters.CharacterSceneObject.Y);
            LogUtils.Log(MessageBuilder.Trace($"Local scene object Id: {characterGameObject.Id} Position: {position}"));

            var obj = AddGameObject(characterGameObject);
            if (obj == null)
            {
                return;
            }

            obj.GetComponent<CharacterCreator>().Create(new CharacterInformation(characterGameObject.Id, character.Name, character.CharacterType));

            localSceneObjectId = characterGameObject.Id;
        }

        private void OnGameObjectAdded(SceneObjectAddedEventParameters parameters)
        {
            var gameObject = parameters.SceneObject;
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

        private void OnGameObjectRemoved(SceneObjectRemovedEventParameters parameters)
        {
            var id = parameters.SceneObjectId;
            RemoveGameObject(id);
        }

        private void OnGameObjectsAdded(SceneObjectsAddedEventParameters parameters)
        {
            var gameObjects = parameters.SceneObjects;
            var ignoreCharacterCreation = new Dictionary<int, bool>();

            foreach (var gameObject in gameObjects)
            {
                var isExists = AddGameObject(gameObject);
                if (isExists == null)
                {
                    ignoreCharacterCreation.Add(gameObject.Id, false);
                }
            }

            foreach (var character in parameters.CharacterInformations)
            {
                if (ignoreCharacterCreation.ContainsKey(character.SceneObjectId))
                {
                    continue;
                }

                var gameObject = GetRemoteGameObject(character.SceneObjectId);
                gameObject?.GetGameObject().GetComponent<CharacterCreator>().Create(character);
            }

            ignoreCharacterCreation.Clear();
        }

        private void OnGameObjectsRemoved(SceneObjectsRemovedEventParameters parameters)
        {
            var ids = parameters.SceneObjectsId;
            foreach (var id in ids)
            {
                RemoveGameObject(id);
            }
        }

        private GameObject AddGameObject(SceneObject gameObject)
        {
            if (sceneObjects.ContainsKey(gameObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"Scene object with id #{gameObject.Id} already exists."), LogMessageType.Warning);
                return null;
            }

            var position = new Vector3(gameObject.X, gameObject.Y);
            var obj = CreateGameObject(gameObject.Name, position);
            if (obj == null)
            {
                return null;
            }

            obj.name = gameObject.Name;

            var networkIdentity = obj.GetComponent<ISceneObject>();
            networkIdentity.Id = gameObject.Id;

            sceneObjects.Add(gameObject.Id, networkIdentity);

            LogUtils.Log(MessageBuilder.Trace($"Added a new scene object with id #{gameObject.Id}"));
            return obj;
        }

        private void RemoveGameObject(int id)
        {
            var gameObject = GetRemoteGameObject(id)?.GetGameObject();
            if (gameObject == null)
            {
                return;
            }

            sceneObjects.Remove(id);

            Destroy(gameObject);

            LogUtils.Log(MessageBuilder.Trace($"Removed a scene object with id #{id}"));
        }

        private GameObject CreateGameObject(string name, Vector3 position)
        {
            const string SCENE_OBJECTS_FOLDER_PATH = "Game/{0}";

            var obj = Resources.Load(string.Format(SCENE_OBJECTS_FOLDER_PATH, name)).AssertNotNull();
            if (obj != null)
            {
                return Instantiate(obj, position, Quaternion.identity) as GameObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with name {name}"), LogMessageType.Error);
            return null;
        }

        public ISceneObject GetLocalGameObject()
        {
            ISceneObject sceneObject;
            if (sceneObjects.TryGetValue(localSceneObjectId, out sceneObject))
            {
                return sceneObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a local scene object with id #{localSceneObjectId}"), LogMessageType.Warning);
            return null;
        }

        public ISceneObject GetRemoteGameObject(int id)
        {
            ISceneObject sceneObject;
            if (sceneObjects.TryGetValue(id, out sceneObject))
            {
                return sceneObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id #{id}"), LogMessageType.Warning);
            return null;
        }
    }
}