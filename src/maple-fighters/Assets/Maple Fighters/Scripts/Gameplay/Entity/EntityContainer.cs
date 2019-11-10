using Game.Common;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.GameEntity
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

        private GameService gameService;

        private void Awake()
        {
            collection = new EntityCollection();
        }

        private void Start()
        {
            gameService = FindObjectOfType<GameService>();
            gameService?.GameSceneApi.SceneEntered.AddListener(OnLocalEntityEntered);
            gameService?.GameSceneApi.SceneObjectAdded.AddListener(OnEntityAdded);
            gameService?.GameSceneApi.SceneObjectRemoved.AddListener(OnEntityRemoved);
            gameService?.GameSceneApi.SceneObjectsAdded.AddListener(OnEntitiesAdded);
            gameService?.GameSceneApi.SceneObjectsRemoved.AddListener(OnEntitiesRemoved);
        }

        private void OnDestroy()
        {
            // TODO: Remove SceneLeft event
            gameService?.GameSceneApi.SceneEntered.RemoveListener(OnLocalEntityEntered);
            gameService?.GameSceneApi.SceneObjectAdded.RemoveListener(OnEntityAdded);
            gameService?.GameSceneApi.SceneObjectRemoved.RemoveListener(OnEntityRemoved);
            gameService?.GameSceneApi.SceneObjectsAdded.RemoveListener(OnEntitiesAdded);
            gameService?.GameSceneApi.SceneObjectsRemoved.RemoveListener(OnEntitiesRemoved);
        }

        private void OnLocalEntityEntered(EnterSceneResponseParameters parameters)
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