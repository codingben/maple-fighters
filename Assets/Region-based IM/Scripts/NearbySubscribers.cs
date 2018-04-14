using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using Scripts.Gameplay;
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

        private void Start()
        {
            var networkIdentity = GetComponent<NetworkIdentity>();

            SubscriberAdded += o => LogUtils.Log($"New subscriber: {o.Id} For Id: {networkIdentity.Id}", LogMessageType.Error);
            SubscriberRemoved += o => LogUtils.Log($"Removed subscriber: {o} For Id: {networkIdentity.Id}", LogMessageType.Error);
            SubscribersAdded += delegate (ISceneObject[] objects) 
            {
                foreach (var sceneObject in objects)
                {
                    LogUtils.Log($"New subscribers: {sceneObject.Id} For Id: {networkIdentity.Id}", LogMessageType.Error);
                }
            };
            SubscribersRemoved += delegate (ISceneObject[] objects) 
            {
                foreach (var sceneObject in objects)
                {
                    LogUtils.Log($"Removed subscribers: {sceneObject} For Id: {networkIdentity.Id}", LogMessageType.Error);
                }
            };
        }

        private void OnDestroy()
        {
            SubscriberAdded = null;
            SubscriberRemoved = null;
            SubscribersAdded = null;
            SubscribersRemoved = null;

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
            var subscribersAdded = sceneObjects.Where(sceneObject => subscribers.Add(sceneObject)).ToList();
            if (subscribersAdded.Count == 0) return;
            {
                SubscribersAdded?.Invoke(subscribersAdded.ToArray());
            }
        }

        public void RemoveSubscribers(IEnumerable<ISceneObject> sceneObjects)
        {
            var subscribersRemoved = sceneObjects.Where(sceneObject => subscribers.Remove(sceneObject)).ToList();
            if (subscribersRemoved.Count == 0) return;
            {
                SubscribersRemoved?.Invoke(subscribersRemoved.ToArray());
            }
        }
    }
}