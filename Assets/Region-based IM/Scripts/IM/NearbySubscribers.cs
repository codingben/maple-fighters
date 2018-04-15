using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InterestManagement.Scripts
{
    public class NearbySubscribers : MonoBehaviour, IInterestAreaEvents
    {
        public event Action<ISceneObject> SubscriberAdded;
        public event Action<ISceneObject> SubscriberRemoved;
        public event Action<ISceneObject[]> SubscribersAdded;
        public event Action<ISceneObject[]> SubscribersRemoved;

        private readonly HashSet<ISceneObject> subscribers = new HashSet<ISceneObject>();

        private void OnDestroy()
        {
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