using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public class NearbySceneObjectsCollection : INearbySceneObjectsEvents
    {
        public event Action<ISceneObject> SceneObjectAdded;

        public event Action<ISceneObject> SceneObjectRemoved;

        public event Action<IEnumerable<ISceneObject>> SceneObjectsAdded;

        public event Action<IEnumerable<ISceneObject>> SceneObjectsRemoved;

        private readonly HashSet<ISceneObject> nearbySceneObjects;

        public NearbySceneObjectsCollection()
        {
            nearbySceneObjects = new HashSet<ISceneObject>();
        }

        public void Dispose()
        {
            nearbySceneObjects?.Clear();
        }

        public void Add(IEnumerable<ISceneObject> sceneObjects)
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

        public void Add(ISceneObject sceneObject)
        {
            if (nearbySceneObjects.Add(sceneObject))
            {
                SceneObjectAdded?.Invoke(sceneObject);
            }
        }

        public void Remove(IEnumerable<ISceneObject> sceneObjects)
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

        public void Remove(ISceneObject sceneObject)
        {
            if (nearbySceneObjects.Remove(sceneObject))
            {
                SceneObjectRemoved?.Invoke(sceneObject);
            }
        }
    }
}