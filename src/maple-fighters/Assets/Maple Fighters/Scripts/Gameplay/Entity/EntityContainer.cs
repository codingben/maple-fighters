using System.Collections.Generic;
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
        private IGameApi gameApi;

        private Dictionary<int, IEntity> collection;

        private void Awake()
        {
            collection = new Dictionary<int, IEntity>();
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

            localEntity = AddEntity(id, name, position);
        }

        private void OnEntitiesAdded(GameObjectsAddedMessage message)
        {
            var gameObjects = message.GameObjects;

            foreach (var gameObject in gameObjects)
            {
                var id = gameObject.Id;
                var name = gameObject.Name;
                var position = new Vector2(gameObject.X, gameObject.Y);

                AddEntity(id, name, position);
            }
        }

        private void OnEntitiesRemoved(GameObjectsRemovedMessage message)
        {
            var identifiers = message.Identifiers;

            foreach (var id in identifiers)
            {
                if (collection.TryGetValue(id, out var entity))
                {
                    RemoveEntity(entity);
                }
            }
        }

        private IEntity AddEntity(int id, string name, Vector2 position)
        {
            IEntity entity = null;

            var gameObject = Utils.CreateGameObject(name, position);
            if (gameObject != null)
            {
                entity = gameObject.GetComponent<IEntity>();

                if (entity != null)
                {
                    entity.Id = id;

                    collection.Add(id, entity);

                    Debug.Log($"Added a new entity with id #{id}");
                }
            }

            return entity;
        }

        private void RemoveEntity(IEntity entity)
        {
            var gameObject = entity.GameObject;
            var id = entity.Id;

            Destroy(gameObject);

            collection.Remove(id);

            Debug.Log($"Removed an entity with id #{id}");
        }

        public IEntity GetLocalEntity()
        {
            return localEntity;
        }

        public bool GetRemoteEntity(int id, out IEntity entity)
        {
            return collection.TryGetValue(id, out entity);
        }
    }
}