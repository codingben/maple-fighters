using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public class NearbySceneObjectsCollection : INearbySceneObjectsCollection
    {
        /// <inheritdoc />
        public event Action<ISceneObject> SceneObjectAdded;

        /// <inheritdoc />
        public event Action<ISceneObject> SceneObjectRemoved;

        /// <inheritdoc />
        public event Action<IEnumerable<ISceneObject>> SceneObjectsAdded;

        /// <inheritdoc />
        public event Action<IEnumerable<ISceneObject>> SceneObjectsRemoved;

        private readonly int excludedId;
        private readonly HashSet<ISceneObject> nearbySceneObjects = new HashSet<ISceneObject>();

        public NearbySceneObjectsCollection(int excludedId)
        {
            this.excludedId = excludedId;
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
                            sceneObject.Id != excludedId
                            && nearbySceneObjects.Add(sceneObject))
                    .ToArray();

            if (visibleSceneObjects.Length != 0)
            {
                SceneObjectsAdded?.Invoke(visibleSceneObjects);
            }
        }

        public void Add(ISceneObject sceneObject)
        {
            if (sceneObject.Id != excludedId
                && nearbySceneObjects.Add(sceneObject))
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
                            sceneObject.Id != excludedId
                            && nearbySceneObjects.Remove(sceneObject))
                    .ToArray();

            if (invisibleSceneObjects.Length != 0)
            {
                SceneObjectsRemoved?.Invoke(invisibleSceneObjects);
            }
        }

        public void Remove(ISceneObject sceneObject)
        {
            if (sceneObject.Id != excludedId
                && nearbySceneObjects.Remove(sceneObject))
            {
                SceneObjectRemoved?.Invoke(sceneObject);
            }
        }
    }
}