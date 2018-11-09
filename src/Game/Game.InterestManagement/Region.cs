using System;
using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public class Region : IRegion
    {
        /// <inheritdoc />
        public event Action<ISceneObject> SubscriberAdded;

        /// <inheritdoc />
        public event Action<ISceneObject> SubscriberRemoved;

        /// <inheritdoc />
        public IRectangle Rectangle { get; }

        private readonly HashSet<ISceneObject> sceneObjects = new HashSet<ISceneObject>();

        public Region(Vector2 position, Vector2 size)
        {
            Rectangle = new Rectangle(position, size);
        }

        public void Subscribe(ISceneObject sceneObject)
        {
            if (sceneObjects.Add(sceneObject))
            {
                SubscriberAdded?.Invoke(sceneObject);
            }
        }

        public void Unsubscribe(ISceneObject sceneObject)
        {
            if (sceneObjects.Remove(sceneObject))
            {
                SubscriberRemoved?.Invoke(sceneObject);
            }
        }

        public IEnumerable<ISceneObject> GetAllSubscribers()
        {
            return sceneObjects;
        }

        public void Dispose()
        {
            SubscriberAdded = null;
            SubscriberRemoved = null;

            sceneObjects?.Clear();
        }
    }
}