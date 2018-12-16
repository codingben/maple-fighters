using System.Collections.Generic;
using CommonTools.Log;
using Game.Common;
using Scripts.Gameplay;
using Scripts.Services;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Containers
{
    public class SceneObjectsContainer : MonoSingleton<SceneObjectsContainer>
    {
        private Dictionary<int, ISceneObject> sceneObjects;
        private int localSceneObjectId;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneObjects = new Dictionary<int, ISceneObject>();

            SubscribeToEvents();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneEntered.AddListener(OnSceneEntered);
            gameScenePeerLogic.SceneObjectAdded.AddListener(OnSceneObjectAdded);
            gameScenePeerLogic.SceneObjectRemoved.AddListener(OnSceneObjectRemoved);
            gameScenePeerLogic.SceneObjectsAdded.AddListener(OnSceneObjectsAdded);
            gameScenePeerLogic.SceneObjectsRemoved.AddListener(OnSceneObjectsRemoved);
        }

        private void UnsubscribeFromEvents()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneEntered.RemoveListener(OnSceneEntered);
            gameScenePeerLogic.SceneObjectAdded.RemoveListener(OnSceneObjectAdded);
            gameScenePeerLogic.SceneObjectRemoved.RemoveListener(OnSceneObjectRemoved);
            gameScenePeerLogic.SceneObjectsAdded.RemoveListener(OnSceneObjectsAdded);
            gameScenePeerLogic.SceneObjectsRemoved.RemoveListener(OnSceneObjectsRemoved);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (sceneObjects.ContainsKey(localSceneObjectId))
            {
                sceneObjects.Remove(localSceneObjectId);
            }
        }

        private void OnSceneEntered(EnterSceneResponseParameters parameters)
        {
            var sceneObject = parameters.SceneObject;
            localSceneObjectId = sceneObject.Id;

            AddSceneObject(sceneObject);

            LogUtils.Log(MessageBuilder.Trace($"Local scene object Id: {sceneObject.Id}"));
        }

        private void OnSceneObjectAdded(SceneObjectAddedEventParameters parameters)
        {
            var sceneObject = parameters.SceneObject;
            AddSceneObject(sceneObject);
        }

        private void OnSceneObjectRemoved(SceneObjectRemovedEventParameters parameters)
        {
            var id = parameters.SceneObjectId;
            RemoveSceneObject(id);
        }

        private void OnSceneObjectsAdded(SceneObjectsAddedEventParameters parameters)
        {
            var sceneObjects = parameters.SceneObjects;
            foreach (var sceneObject in sceneObjects)
            {
                AddSceneObject(sceneObject);
            }
        }

        private void OnSceneObjectsRemoved(SceneObjectsRemovedEventParameters parameters)
        {
            var ids = parameters.SceneObjectsId;
            foreach (var id in ids)
            {
                RemoveSceneObject(id);
            }
        }

        private void AddSceneObject(SceneObjectParameters parameters)
        {
            var id = parameters.Id;
            var name = parameters.Name;

            if (sceneObjects.ContainsKey(id))
            {
                LogUtils.Log(MessageBuilder.Trace($"Scene object with id #{id} already exists."), LogMessageType.Warning);
                return;
            }

            var sceneObject = CreateSceneObject(name, new Vector3(parameters.X, parameters.Y));
            if (sceneObject == null)
            {
                return;
            }

            sceneObject.name = name;

            var sceneObjectComponent = sceneObject.GetComponent<ISceneObject>();
            sceneObjectComponent.Id = id;

            sceneObjects.Add(id, sceneObjectComponent);

            LogUtils.Log(MessageBuilder.Trace($"Added a new scene object with id #{id}"));
        }

        private void RemoveSceneObject(int id)
        {
            var sceneObject = GetRemoteSceneObject(id)?.GetGameObject();
            if (sceneObject == null)
            {
                return;
            }

            sceneObjects.Remove(id);

            Destroy(sceneObject);

            LogUtils.Log(MessageBuilder.Trace($"Removed a scene object with id #{id}"));
        }

        private GameObject CreateSceneObject(string name, Vector3 position)
        {
            const string SCENE_OBJECTS_FOLDER_PATH = "Game/{0}";

            var sceneObject = Resources.Load(string.Format(SCENE_OBJECTS_FOLDER_PATH, name)).AssertNotNull($"Could not find {name} scene object.");
            if (sceneObject != null)
            {
                var gameObject = Instantiate(sceneObject, position, Quaternion.identity) as GameObject;
                if (gameObject != null)
                {
                    return gameObject;
                }

                LogUtils.Log(MessageBuilder.Trace($"Could not create a scene object with name {name}"), LogMessageType.Error);
                return null;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with name {name}"), LogMessageType.Error);
            return null;
        }

        public ISceneObject GetLocalSceneObject()
        {
            ISceneObject sceneObject;
            if (sceneObjects.TryGetValue(localSceneObjectId, out sceneObject))
            {
                return sceneObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a local scene object with id #{localSceneObjectId}"), LogMessageType.Warning);
            return null;
        }

        public ISceneObject GetRemoteSceneObject(int id)
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