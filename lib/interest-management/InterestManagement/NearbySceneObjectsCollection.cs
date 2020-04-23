using System;
using System.Collections.Generic;
using System.Linq;

namespace InterestManagement
{
    public class NearbySceneObjectsCollection<TObject> : INearbySceneObjectsEvents<TObject>
        where TObject : ISceneObject
    {
        public event Action<TObject> SceneObjectAdded;

        public event Action<TObject> SceneObjectRemoved;

        public event Action<IEnumerable<TObject>> SceneObjectsAdded;

        public event Action<IEnumerable<TObject>> SceneObjectsRemoved;

        private readonly HashSet<TObject> collection;

        public NearbySceneObjectsCollection()
        {
            collection = new HashSet<TObject>();
        }

        public void Dispose()
        {
            collection?.Clear();
        }

        public void Add(IEnumerable<TObject> sceneObjects)
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

        public void Add(TObject sceneObject)
        {
            if (collection.Add(sceneObject))
            {
                SceneObjectAdded?.Invoke(sceneObject);
            }
        }

        public void Remove(IEnumerable<TObject> sceneObjects)
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

        public void Remove(TObject sceneObject)
        {
            if (collection.Remove(sceneObject))
            {
                SceneObjectRemoved?.Invoke(sceneObject);
            }
        }
    }
}