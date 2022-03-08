using System.Collections;
using System.Collections.Generic;
using Game.Messages;
using Scripts.Gameplay.Graphics;
using Scripts.Gameplay.Player;
using Scripts.Services;
using Scripts.Services.GameApi;
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
            gameApi = ApiProvider.ProvideGameApi();
            gameApi.SceneEntered.AddListener(OnSceneEntered);
            gameApi.GameObjectsAdded.AddListener(OnGameObjectsAdded);
            gameApi.GameObjectsRemoved.AddListener(OnGameObjectsRemoved);
        }

        private void OnDisable()
        {
            gameApi?.SceneEntered?.RemoveListener(OnSceneEntered);
            gameApi?.GameObjectsAdded?.RemoveListener(OnGameObjectsAdded);
            gameApi?.GameObjectsRemoved?.RemoveListener(OnGameObjectsRemoved);
        }

        private void OnSceneEntered(EnteredSceneMessage message)
        {
            var name = "LocalPlayer";
            var id = message.GameObjectId;
            var x = message.X;
            var y = message.Y;
            var position = new Vector2(x, y);
            var direction = message.Direction;

            localEntity = AddEntity(id, name, position, direction);
        }

        private void OnGameObjectsAdded(GameObjectsAddedMessage message)
        {
            var gameObjects = message.GameObjects;

            foreach (var gameObject in gameObjects)
            {
                var id = gameObject.Id;
                var name = gameObject.Name;
                var position = new Vector2(gameObject.X, gameObject.Y);
                var direction = gameObject.Direction;

                if (collection.ContainsKey(id))
                {
                    Debug.LogWarning($"The entity with id #{id} already exists.");
                }
                else
                {
                    AddEntity(id, name, position, direction);
                }
            }
        }

        private void OnGameObjectsRemoved(GameObjectsRemovedMessage message)
        {
            var gameObjectIds = message.GameObjectIds;

            foreach (var id in gameObjectIds)
            {
                if (collection.TryGetValue(id, out var entity))
                {
                    RemoveEntity(entity);
                }
            }
        }

        private IEntity AddEntity(
            int id,
            string name,
            Vector2 position,
            float direction)
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

                    StartCoroutine(SetEntityDirection(gameObject, direction));
                }
            }

            return entity;
        }

        private void RemoveEntity(IEntity entity)
        {
            var gameObject = entity.GameObject;
            var id = entity.Id;
            var fadeEffectProvider =
                gameObject.GetComponent<IFadeEffectProvider>();
            if (fadeEffectProvider != null)
            {
                var fadeEffect = fadeEffectProvider.Provide();
                if (fadeEffect != null)
                {
                    fadeEffect.UnFadeAndDestroyGameObject();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }

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

        private IEnumerator SetEntityDirection(GameObject entity, float direction)
        {
            yield return new WaitForSeconds(0.1f);

            if (entity == null || direction == 0)
            {
                yield break;
            }

            if (entity.name == "LocalPlayer" || entity.name == "RemotePlayer")
            {
                var spawnedCharacter = entity.GetComponent<ISpawnedCharacter>();
                var character = spawnedCharacter?.GetCharacter();
                if (character != null)
                {
                    var x = direction;
                    var y = character.transform.localScale.y;
                    var z = character.transform.localScale.z;

                    character.transform.localScale = new Vector3(x, y, z);
                }
            }
            else
            {
                var x = direction;
                var y = entity.transform.localScale.y;
                var z = entity.transform.localScale.z;

                entity.transform.localScale = new Vector3(x, y, z);
            }
        }
    }
}