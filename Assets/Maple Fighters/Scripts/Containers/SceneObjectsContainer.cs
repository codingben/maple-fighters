using System.Collections.Generic;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Gameplay;
using Scripts.Utils;
using Game.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Containers
{
    public class SceneObjectsContainer : DontDestroyOnLoad<SceneObjectsContainer>
    {
        private readonly Dictionary<int, ISceneObject> sceneObjects = new Dictionary<int, ISceneObject>();
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private int localSceneObjectId;

        protected override void OnAwake()
        {
            base.OnAwake();

            SubscribeToEvents();
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.Dispose();

            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            ServiceContainer.GameService.SceneEntered.AddListener(OnSceneEntered);
            ServiceContainer.GameService.SceneObjectAdded.AddListener(OnSceneObjectAdded);
            ServiceContainer.GameService.SceneObjectRemoved.AddListener(OnSceneObjectRemoved);
            ServiceContainer.GameService.SceneObjectsAdded.AddListener(OnSceneObjectsAdded);
            ServiceContainer.GameService.SceneObjectsRemoved.AddListener(OnSceneObjectsRemoved);
        }

        private void UnsubscribeFromEvents()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            ServiceContainer.GameService.SceneEntered.RemoveListener(OnSceneEntered);
            ServiceContainer.GameService.SceneObjectAdded.RemoveListener(OnSceneObjectAdded);
            ServiceContainer.GameService.SceneObjectRemoved.RemoveListener(OnSceneObjectRemoved);
            ServiceContainer.GameService.SceneObjectsAdded.RemoveListener(OnSceneObjectsAdded);
            ServiceContainer.GameService.SceneObjectsRemoved.RemoveListener(OnSceneObjectsRemoved);
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
            AddSceneObject(sceneObject);

            localSceneObjectId = sceneObject.Id;

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

        private void AddSceneObject(SceneObjectParameters sceneObject)
        {
            if (sceneObjects.ContainsKey(sceneObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"Scene object with id #{sceneObject.Id} already exists."), LogMessageType.Warning);
                return;
            }

            var position = new Vector3(sceneObject.X, sceneObject.Y);
            var obj = CreateSceneObject(sceneObject.Name, position);
            if (obj == null)
            {
                return;
            }

            obj.name = sceneObject.Name;

            var networkIdentity = obj.GetComponent<ISceneObject>();
            networkIdentity.Id = sceneObject.Id;

            sceneObjects.Add(sceneObject.Id, networkIdentity);

            LogUtils.Log(MessageBuilder.Trace($"Added a new scene object with id #{sceneObject.Id}"));
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

            var obj = Resources.Load(string.Format(SCENE_OBJECTS_FOLDER_PATH, name)).AssertNotNull();
            if (obj != null)
            {
                var sceneObject = Instantiate(obj, position, Quaternion.identity) as GameObject;
                if (sceneObject != null)
                {
                    return sceneObject;
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