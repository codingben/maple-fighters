using System;
using System.Collections.Generic;
using System.Linq;
using ComponentModel.Common;
using InterestManagement.Components.Interfaces;

namespace InterestManagement.Components
{
    public class NearbySubscribers : Component<ISceneObject>, IInterestAreaEvents
    {
        public event Action<ISceneObject> SubscriberAdded;
        public event Action<ISceneObject> SubscriberRemoved;
        public event Action<ISceneObject[]> SubscribersAdded;
        public event Action<ISceneObject[]> SubscribersRemoved;

        private readonly HashSet<ISceneObject> subscribers = new HashSet<ISceneObject>();

        protected override void OnDestroy()
        {
            base.OnDestroy();

            subscribers.Clear();
        }

        public void AddSubscriber(ISceneObject sceneObject)
        {
            if (subscribers.Add(sceneObject))
            {
                SubscriberAdded?.Invoke(sceneObject);
            }
        }

        public void RemoveSubscriber(ISceneObject sceneObject)
        {
            if (subscribers.Remove(sceneObject))
            {
                SubscriberRemoved?.Invoke(sceneObject);
            }
        }

        public void AddSubscribers(IEnumerable<ISceneObject> sceneObjects)
        {
            var subscribersAdded = sceneObjects.Where(sceneObject => subscribers.Add(sceneObject)).ToArray();
            if (subscribersAdded.Length == 0) return;
            {
                SubscribersAdded?.Invoke(subscribersAdded);
            }
        }

        public void RemoveSubscribers(IEnumerable<ISceneObject> sceneObjects)
        {
            var subscribersRemoved = sceneObjects.Where(sceneObject => subscribers.Remove(sceneObject)).ToArray();
            if (subscribersRemoved.Length == 0) return;
            {
                SubscribersRemoved?.Invoke(subscribersRemoved);
            }
        }
    }
}