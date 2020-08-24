using System;
using System.Collections.Generic;
using MathematicsHelper;

namespace InterestManagement
{
    public struct Region<TSceneObject> : IRegion<TSceneObject>
        where TSceneObject : ISceneObject
    {
        public event Action<TSceneObject> SubscriberAdded;

        public event Action<TSceneObject> SubscriberRemoved;

        private Rectangle rectangle;
        private readonly HashSet<TSceneObject> sceneObjects;

        public Region(Vector2 position, Vector2 size)
        {
            SubscriberAdded = null;
            SubscriberRemoved = null;

            rectangle = new Rectangle(position, size);
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
                SubscriberAdded?.Invoke(sceneObject);
            }
        }

        public void Unsubscribe(TSceneObject sceneObject)
        {
            if (sceneObjects.Remove(sceneObject))
            {
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

        public bool IsOverlaps(Vector2 position, Vector2 size)
        {
            return rectangle.Intersects(position, size);
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