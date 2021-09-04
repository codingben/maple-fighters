using System;
using System.Collections.Generic;
using Common.MathematicsHelper;

namespace InterestManagement
{
    public struct Region<TSceneObject> : IRegion<TSceneObject>
        where TSceneObject : ISceneObject
    {
        public event Action<TSceneObject> SubscriberAdded;

        public event Action<TSceneObject> SubscriberRemoved;

        private readonly ILogger log;
        private readonly Rectangle rectangle;
        private readonly HashSet<TSceneObject> sceneObjects;

        public Region(Rectangle rectangle, ILogger log = null)
        {
            this.log = log;
            this.rectangle = rectangle;

            SubscriberAdded = null;
            SubscriberRemoved = null;

            sceneObjects = new HashSet<TSceneObject>();
        }

        public void Dispose()
        {
            SubscriberAdded = null;
            SubscriberRemoved = null;

            sceneObjects?.Clear();
        }

        public void Subscribe(TSceneObject sceneObject)
        {
            if (sceneObjects.Add(sceneObject))
            {
                log?.Info($"Scene object #{sceneObject.Id} subscribed to new region");

                SubscriberAdded?.Invoke(sceneObject);
            }
        }

        public void Unsubscribe(TSceneObject sceneObject)
        {
            if (sceneObjects.Remove(sceneObject))
            {
                log?.Info($"Scene object #{sceneObject.Id} unsubscribed from old region");

                SubscriberRemoved?.Invoke(sceneObject);
            }
        }

        public IEnumerable<TSceneObject> GetAllSubscribers()
        {
            return sceneObjects;
        }

        public int SubscriberCount()
        {
            return sceneObjects.Count;
        }

        public bool IsOverlaps(ITransform transform)
        {
            return rectangle.Intersects(transform.Position, transform.Size);
        }

        public Vector2 GetPosition()
        {
            return rectangle.Position;
        }

        public Vector2 GetSize()
        {
            return rectangle.Size;
        }
    }
}