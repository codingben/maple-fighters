using System;
using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public struct Region<TObject> : IRegion<TObject>
        where TObject : ISceneObject
    {
        public event Action<TObject> SubscriberAdded;

        public event Action<TObject> SubscriberRemoved;

        private Rectangle rectangle;
        private readonly HashSet<TObject> sceneObjects;

        public Region(Vector2 position, Vector2 size)
        {
            SubscriberAdded = null;
            SubscriberRemoved = null;

            rectangle = new Rectangle(position, size);
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