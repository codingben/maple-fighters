using System.Collections.Generic;
using Game.Common;
using Scripts.Gameplay;
using UnityEngine;

namespace Scripts.Containers
{
    public class EntitiesContainer : MonoBehaviour, IEntitiesContainer
    {
        public static EntitiesContainer GetInstance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EntitiesContainer>();
            }

            return instance;
        }

        private static EntitiesContainer instance;

        private IEntity localEntity;
        private Dictionary<int, IEntity> collection;

        private void Awake()
        {
            collection = new Dictionary<int, IEntity>();
        }

        private void OnDestroy()
        {
            collection.Clear();
        }

        public void SetLocalEntity(IEntity entity)
        {
            localEntity = entity;
        }

        public IEntity AddEntity(SceneObjectParameters parameters)
        {
            var id = parameters.Id;
            var name = parameters.Name;

            if (collection.ContainsKey(id))
            {
                Debug.LogWarning($"Scene object with id #{id} already exists.");
            }
            else
            {
                var gameObject = CreateGameObject(
                    name,
                    new Vector3(parameters.X, parameters.Y));
                if (gameObject != null)
                {
                    var entity = gameObject.GetComponent<IEntity>();
                    entity.Id = id;

                    collection.Add(id, entity);

                    Debug.Log($"Added a new scene object with id #{id}");
                }
            }

            return collection[id];
        }

        public void RemoveEntity(int id)
        {
            var entity = GetRemoteEntity(id)?.GameObject;
            if (entity != null)
            {
                collection.Remove(id);

                Destroy(entity);

                Debug.Log($"Removed a scene object with id #{id}");
            }
        }

        public IEntity GetLocalEntity()
        {
            return localEntity;
        }

        public IEntity GetRemoteEntity(int id)
        {
            if (!collection.TryGetValue(id, out var entity))
            {
                Debug.LogWarning(
                    $"Could not find a scene object with id #{id}");
            }

            return entity;
        }

        private GameObject CreateGameObject(string name, Vector3 position)
        {
            GameObject gameObject = null;

            var entity = Resources.Load($"Game/{name}");
            if (entity != null)
            {
                gameObject = 
                    Instantiate(entity, position, Quaternion.identity) 
                        as GameObject;

                if (gameObject != null)
                {
                    gameObject.name = name;
                }
            }
            else
            {
                Debug.LogError(
                    $"Could not find an object with name {name}");
            }

            return gameObject;
        }
    }
}