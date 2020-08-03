using Game.Common;
using Game.Messages;
using Scripts.Services.Game;
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
        private IGameApi gameApi;

        private void Awake()
        {
            collection = new EntityCollection();
        }

        private void Start()
        {
            gameApi = FindObjectOfType<GameApi>();
            gameApi.SceneEntered += OnLocalEntityEntered;
            gameApi.SceneObjectsAdded += OnEntitiesAdded;
            gameApi.SceneObjectsRemoved += OnEntitiesRemoved;
        }

        private void OnDisable()
        {
            // TODO: Remove SceneLeft event
            gameApi.SceneEntered -= OnLocalEntityEntered;
            gameApi.SceneObjectsAdded -= OnEntitiesAdded;
            gameApi.SceneObjectsRemoved -= OnEntitiesRemoved;
        }

        private void OnLocalEntityEntered(EnteredSceneMessage message)
        {
            // TODO: Remove "Local Player"
            var name = "Local Player";
            var id = message.GameObjectId;
            var x = message.SpawnPositionData.X;
            var y = message.SpawnPositionData.Y;
            var position = new Vector2(x, y);

            localEntity = collection.Add(id, name, position);
        }

        private void OnEntitiesAdded(GameObjectsAddedMessage message)
        {
            var gameObjects = message.GameObjects;

            foreach (var gameObject in gameObjects)
            {
                var id = gameObject.Id;
                var name = gameObject.Name;
                var position = new Vector2(gameObject.X, gameObject.Y);

                collection.Add(id, name, position);
            }
        }

        private void OnEntitiesRemoved(GameObjectsRemovedMessage message)
        {
            var identifiers = message.Identifiers;

            foreach (var id in identifiers)
            {
                collection.Remove(id);
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