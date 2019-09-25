using Game.Common;
using Scripts.Network.Services;
using UnityEngine;

namespace Scripts.Gameplay.Entity
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
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
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
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
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
            var id = parameters.SceneObject.Id;
            var name = parameters.SceneObject.Name;
            var position = 
                new Vector2(parameters.SceneObject.X, parameters.SceneObject.Y);

            localEntity = collection.Add(id, name, position);
        }

        private void OnEntityAdded(SceneObjectAddedEventParameters parameters)
        {
            var id = parameters.SceneObject.Id;
            var name = parameters.SceneObject.Name;
            var position =
                new Vector2(parameters.SceneObject.X, parameters.SceneObject.Y);

            collection.Add(id, name, position);
        }

        private void OnEntityRemoved(SceneObjectRemovedEventParameters parameters)
        {
            collection.Remove(parameters.SceneObjectId);
        }

        private void OnEntitiesAdded(SceneObjectsAddedEventParameters parameters)
        {
            foreach (var sceneObject in parameters.SceneObjects)
            {
                var id = sceneObject.Id;
                var name = sceneObject.Name;
                var position = new Vector2(sceneObject.X, sceneObject.Y);

                collection.Add(id, name, position);
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