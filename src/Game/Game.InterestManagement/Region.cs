using System;
using System.Collections.Generic;
using System.Linq;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public struct Region<TObject> : IRegion<TObject>
        where TObject : ISceneObject
    {
        public IRectangle Rectangle { get; }

        public event Action<TObject> SubscriberAdded;

        public event Action<TObject> SubscriberRemoved;

        private readonly HashSet<TObject> sceneObjects;

        public Region(Vector2 position, Vector2 size)
        {
            Rectangle = new Rectangle(position, size);

            SubscriberAdded = null;
            SubscriberRemoved = null;

            sceneObjects = new HashSet<TObject>();
        }

        public void Dispose()
        {
            SubscriberAdded = null;
            SubscriberRemoved = null;

            sceneObjects?.Clear();
        }

        public void Subscribe(TObject sceneObject)
        {
            if (sceneObjects.Add(sceneObject))
            {
                SubscriberAdded?.Invoke(sceneObject);
            }
        }

        public void Unsubscribe(TObject sceneObject)
        {
            if (sceneObjects.Remove(sceneObject))
            {
                SubscriberRemoved?.Invoke(sceneObject);
            }
        }

        public IEnumerable<TObject> GetAllSubscribers()
        {
            return sceneObjects;
        }

        public bool HasSubscribers()
        {
            return sceneObjects != null && sceneObjects.Any();
        }
    }
}