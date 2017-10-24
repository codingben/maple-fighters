using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using Scripts.Gameplay;
using UnityEngine;

namespace InterestManagement.Scripts
{
    public class InterestArea : MonoBehaviour, IInterestArea
    {
        public event Action<ISceneObject> SubscriberAdded;
        public event Action<int> SubscriberRemoved;
        public event Action<ISceneObject[]> SubscribersAdded;
        public event Action<int[]> SubscribersRemoved;

        private readonly Dictionary<int, ISceneObject> interestedSceneObjects = new Dictionary<int, ISceneObject>();

        private Scene scene;
        private NetworkIdentity networkIdentity;

        private Rectangle interestArea;

        private void Awake()
        {
            scene = FindObjectOfType<Scene>().AssertNotNull();

            interestArea = new Rectangle(transform.position, scene.RegionSize);

            networkIdentity = GetComponent<NetworkIdentity>();
            networkIdentity.Id = gameObject.GetInstanceID();

            name = networkIdentity.Id.ToString();
        }

        private void Start()
        {
            SubscriberAdded += o => LogUtils.Log($"New subscriber: {o.Id} For Id: {networkIdentity.Id}", LogMessageType.Error);
            SubscriberRemoved += o => LogUtils.Log($"Removed subscriber: {o} For Id: {networkIdentity.Id}", LogMessageType.Error);
            SubscribersAdded += delegate(ISceneObject[] objects) {
                foreach (var sceneObject in objects)
                {
                    LogUtils.Log($"New subscribers: {sceneObject.Id} For Id: {networkIdentity.Id}", LogMessageType.Error);
                }
            };
            SubscribersRemoved += delegate (int[] objects) {
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

            interestedSceneObjects.Clear();
        }

        private void Update()
        {
            SetPosition(transform.position);
        }

        private void SetPosition(Vector2 position)
        {
            interestArea.SetPosition(position);

            if (scene != null)
            {
                DetectOverlapsWithRegions();
            }
        }

        public IEnumerable<IRegion> GetSubscribedPublishers()
        {
            var regions = scene.Regions;
            return regions.Cast<IRegion>().Where(region => region.HasSubscription(networkIdentity.Id)).ToArray();
        }

        public void DetectOverlapsWithRegions()
        {
            var sceneRegions = scene.Regions;

            foreach (var region in sceneRegions)
            {
                if (region == null)
                {
                    LogUtils.Log(MessageBuilder.Trace("Region is null."));
                    continue;
                }

                if (!Rectangle.Intersect(region.PublisherArea, interestArea).Equals(Rectangle.EMPTY))
                {
                    if (region.HasSubscription(networkIdentity.Id))
                    {
                        continue;
                    }

                    region.AddSubscription(networkIdentity);
                }
                else
                {
                    if (region.HasSubscription(networkIdentity.Id))
                    {
                        region.RemoveSubscription(networkIdentity.Id);
                    }
                }
            }
        }

        public void InvokeSubscriberAdded(ISceneObject sceneObject)
        {
            if (interestedSceneObjects.ContainsKey(sceneObject.Id))
            {
                return;
            }

            interestedSceneObjects.Add(sceneObject.Id, sceneObject);

            SubscriberAdded?.Invoke(sceneObject);
        }

        public void InvokeSubscriberRemoved(int sceneObjectId)
        {
            if (!interestedSceneObjects.ContainsKey(sceneObjectId))
            {
                return;
            }

            interestedSceneObjects.Remove(sceneObjectId);

            SubscriberRemoved?.Invoke(sceneObjectId);
        }

        public void InvokeSubscribersAdded(ISceneObject[] sceneObjects)
        {
            var subscribersAdded = new List<ISceneObject>();

            foreach (var sceneObject in sceneObjects)
            {
                if (interestedSceneObjects.ContainsKey(sceneObject.Id))
                {
                    continue;
                }

                subscribersAdded.Add(sceneObject);
                interestedSceneObjects.Add(sceneObject.Id, sceneObject);
            }

            if (subscribersAdded.Count == 0) return;
            {
                SubscribersAdded?.Invoke(sceneObjects);
            }
        }

        public void InvokeSubscribersRemoved(int[] sceneObjectIds)
        {
            var subscribersRemoved = new List<int>();

            foreach (var id in sceneObjectIds)
            {
                if (!interestedSceneObjects.ContainsKey(id))
                {
                    continue;
                }

                subscribersRemoved.Add(id);
                interestedSceneObjects.Remove(id);
            }

            if (subscribersRemoved.Count == 0) return;
            {
                SubscribersRemoved?.Invoke(sceneObjectIds);
            }
        }
    }
}