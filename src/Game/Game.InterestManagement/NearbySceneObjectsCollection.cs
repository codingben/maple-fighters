using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public class NearbySceneObjectsCollection<TObject> : INearbySceneObjectsEvents<TObject>
        where TObject : ISceneObject
    {
        public event Action<TObject> SceneObjectAdded;

        public event Action<TObject> SceneObjectRemoved;

        public event Action<IEnumerable<TObject>> SceneObjectsAdded;

        public event Action<IEnumerable<TObject>> SceneObjectsRemoved;

        private readonly HashSet<TObject> nearbySceneObjects;

        public NearbySceneObjectsCollection()
        {
            nearbySceneObjects = new HashSet<TObject>();
        }

        public void Dispose()
        {
            nearbySceneObjects?.Clear();
        }

        public void Add(IEnumerable<TObject> sceneObjects)
        {
            var visibleSceneObjects =
                sceneObjects
                    .Where(
                        sceneObject =>
                            nearbySceneObjects.Add(sceneObject))
                    .ToArray();

            if (visibleSceneObjects.Length != 0)
            {
                SceneObjectsAdded?.Invoke(visibleSceneObjects);
            }
        }

        public void Add(TObject sceneObject)
        {
            if (nearbySceneObjects.Add(sceneObject))
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
                            nearbySceneObjects.Remove(sceneObject))
                    .ToArray();

            if (invisibleSceneObjects.Length != 0)
            {
                SceneObjectsRemoved?.Invoke(invisibleSceneObjects);
            }
        }

        public void Remove(TObject sceneObject)
        {
            if (nearbySceneObjects.Remove(sceneObject))
            {
                SceneObjectRemoved?.Invoke(sceneObject);
            }
        }
    }
}