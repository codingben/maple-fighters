using System;
using System.Collections.Generic;
using System.Linq;

namespace InterestManagement
{
    public class NearbySceneObjectsCollection<TSceneObject> : INearbySceneObjectsEvents<TSceneObject>
        where TSceneObject : ISceneObject
    {
        public event Action<TSceneObject> SceneObjectAdded;

        public event Action<TSceneObject> SceneObjectRemoved;

        public event Action<IEnumerable<TSceneObject>> SceneObjectsAdded;

        public event Action<IEnumerable<TSceneObject>> SceneObjectsRemoved;

        private readonly HashSet<TSceneObject> collection;

        public NearbySceneObjectsCollection()
        {
            collection = new HashSet<TSceneObject>();
        }

        public void Add(IEnumerable<TSceneObject> sceneObjects)
        {
            var visibleSceneObjects =
                sceneObjects
                    .Where(
                        sceneObject =>
                            collection.Add(sceneObject))
                    .ToArray();

            if (visibleSceneObjects.Length != 0)
            {
                SceneObjectsAdded?.Invoke(visibleSceneObjects);
            }
        }

        public void Add(TSceneObject sceneObject)
        {
            if (collection.Add(sceneObject))
            {
                SceneObjectAdded?.Invoke(sceneObject);
            }
        }

        public void Remove(IEnumerable<TSceneObject> sceneObjects)
        {
            var invisibleSceneObjects =
                sceneObjects
                    .Where(
                        sceneObject =>
                            collection.Remove(sceneObject))
                    .ToArray();

            if (invisibleSceneObjects.Length != 0)
            {
                SceneObjectsRemoved?.Invoke(invisibleSceneObjects);
            }
        }

        public void Remove(TSceneObject sceneObject)
        {
            if (collection.Remove(sceneObject))
            {
                SceneObjectRemoved?.Invoke(sceneObject);
            }
        }

        public void Clear()
        {
            collection?.Clear();
        }

        public IEnumerable<TSceneObject> GetSceneObjects()
        {
            return collection.ToArray();
        }
    }
}