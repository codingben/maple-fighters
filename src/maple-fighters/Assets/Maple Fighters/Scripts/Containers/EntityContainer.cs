using Game.Common;
using Scripts.Gameplay;
using UnityEngine;

namespace Scripts.Containers
{
    public class EntityContainer : MonoBehaviour
    {
        public static EntityContainer GetInstance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EntityContainer>();
            }

            return instance;
        }

        private static EntityContainer instance;

        private IEntity localEntity;
        private IEntityCollection collection;

        private void Awake()
        {
            collection = new EntityCollection();
        }

        private void Start()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                // TODO: Add SceneLeft event
                gameSceneApi.SceneEntered.AddListener(OnEntityEntered);
                gameSceneApi.SceneObjectAdded.AddListener(OnEntityAdded);
                gameSceneApi.SceneObjectRemoved.AddListener(OnEntityRemoved);
                gameSceneApi.SceneObjectsAdded.AddListener(OnEntitiesAdded);
                gameSceneApi.SceneObjectsRemoved.AddListener(OnEntitiesRemoved);
            }
        }

        private void OnDestroy()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                // TODO: Remove SceneLeft event
                gameSceneApi.SceneEntered.RemoveListener(OnEntityEntered);
                gameSceneApi.SceneObjectAdded.RemoveListener(OnEntityAdded);
                gameSceneApi.SceneObjectRemoved.RemoveListener(OnEntityRemoved);
                gameSceneApi.SceneObjectsAdded.RemoveListener(OnEntitiesAdded);
                gameSceneApi.SceneObjectsRemoved.RemoveListener(OnEntitiesRemoved);
            }
        }

        private void OnEntityEntered(EnterSceneResponseParameters parameters)
        {
            localEntity = collection.Add(parameters.SceneObject);
        }

        private void OnEntityAdded(SceneObjectAddedEventParameters parameters)
        {
            collection.Add(parameters.SceneObject);
        }

        private void OnEntityRemoved(SceneObjectRemovedEventParameters parameters)
        {
            collection.Remove(parameters.SceneObjectId);
        }

        private void OnEntitiesAdded(SceneObjectsAddedEventParameters parameters)
        {
            foreach (var sceneObject in parameters.SceneObjects)
            {
                collection.Add(sceneObject);
            }
        }

        private void OnEntitiesRemoved(SceneObjectsRemovedEventParameters parameters)
        {
            foreach (var sceneObjectId in parameters.SceneObjectsId)
            {
                collection.Remove(sceneObjectId);
            }
        }

        public IEntity GetLocalEntity()
        {
            return localEntity;
        }

        public IEntity GetRemoteEntity(int id)
        {
            return collection.TryGet(id);
        }
    }
}