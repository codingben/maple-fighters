using System;
using System.Collections.Generic;
using Game.Common;
using Scripts.Gameplay;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Containers
{
    public class EntityCollection : IDisposable
    {
        private readonly Dictionary<int, IEntity> collection;

        public EntityCollection()
        {
            collection = new Dictionary<int, IEntity>();
        }

        public void Dispose()
        {
            collection.Clear();
        }

        public IEntity Add(SceneObjectParameters parameters)
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

        public void Remove(int id)
        {
            var entity = GetEntity(id);
            if (entity != null)
            {
                Object.Destroy(entity.GameObject);

                collection.Remove(id);

                Debug.Log($"Removed a scene object with id #{id}");
            }
        }

        public IEntity GetEntity(int id)
        {
            if (!collection.TryGetValue(id, out var entity))
            {
                Debug.LogWarning(
                    $"Could not find a scene object with id #{id}");
            }

            return entity;
        }
    }
}