using System;
using System.Collections.Generic;
using System.Linq;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public struct Region : IRegion
    {
        public IRectangle Rectangle { get; }

        public event Action<ISceneObject> SubscriberAdded;

        public event Action<ISceneObject> SubscriberRemoved;

        private readonly HashSet<ISceneObject> sceneObjects;

        public Region(Vector2 position, Vector2 size)
        {
            Rectangle = new Rectangle(position, size);

            SubscriberAdded = null;
            SubscriberRemoved = null;

            sceneObjects = new HashSet<ISceneObject>();
        }

        public void Dispose()
        {
            SubscriberAdded = null;
            SubscriberRemoved = null;

            sceneObjects?.Clear();
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

        public bool HasSubscribers()
        {
            return sceneObjects != null && sceneObjects.Any();
        }
    }
}