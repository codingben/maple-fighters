using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using ComponentModel.Common;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;

namespace InterestManagement.Components
{
    public class InterestArea : Component<ISceneObject>, IInterestArea, IInterestAreaEvents
    {
        public event Action<ISceneObject> SubscriberAdded;
        public event Action<int> SubscriberRemoved;
        public event Action<ISceneObject[]> SubscribersAdded;
        public event Action<int[]> SubscribersRemoved;

        public void AddSubscriber(ISceneObject sceneObject)
        {
            if (subscribers.ContainsKey(sceneObject.Id))
            {
                return;
            }

            subscribers.Add(sceneObject.Id, sceneObject);

            SubscriberAdded?.Invoke(sceneObject);
        }

        public void RemoveSubscriber(int sceneObjectId)
        {
            if (!subscribers.ContainsKey(sceneObjectId))
            {
                return;
            }

            subscribers.Remove(sceneObjectId);

            SubscriberRemoved?.Invoke(sceneObjectId);
        }

        public void AddSubscribers(IEnumerable<ISceneObject> sceneObjects)
        {
            var subscribersAdded = new List<ISceneObject>();

            foreach (var sceneObject in sceneObjects)
            {
                if (subscribers.ContainsKey(sceneObject.Id))
                {
                    continue;
                }

                subscribersAdded.Add(sceneObject);
                subscribers.Add(sceneObject.Id, sceneObject);
            }

            if (subscribersAdded.Count == 0) return;
            {
                SubscribersAdded?.Invoke(subscribersAdded.ToArray());
            }
        }

        public void RemoveSubscribers(IEnumerable<int> sceneObjectIds)
        {
            var subscribersRemoved = new List<int>();

            foreach (var id in sceneObjectIds)
            {
                if (!subscribers.ContainsKey(id))
                {
                    continue;
                }

                subscribersRemoved.Add(id);
                subscribers.Remove(id);
            }

            if (subscribersRemoved.Count == 0) return;
            {
                SubscribersRemoved?.Invoke(subscribersRemoved.ToArray());
            }
        }

        private readonly Dictionary<int, ISceneObject> subscribers = new Dictionary<int, ISceneObject>();

        private Rectangle interestArea;
        private IPresenceSceneProvider presenceSceneProvider;

        public InterestArea(Vector2 position, Vector2 areaSize)
        {
            interestArea = new Rectangle(position, areaSize);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            presenceSceneProvider = Entity.Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();

            SubscribeToPositionChanged();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            subscribers.Clear();

            UnubscribeFromPositionChanged();
        }

        private void SubscribeToPositionChanged()
        {
            var transform = Entity.Components.GetComponent<IPositionTransform>().AssertNotNull();
            transform.PositionChanged += OnPositionChanged;
        }

        private void UnubscribeFromPositionChanged()
        {
            var transform = Entity.Components.GetComponent<IPositionTransform>().AssertNotNull();
            transform.PositionChanged -= OnPositionChanged;
        }

        private void OnPositionChanged(Vector2 position)
        {
            interestArea.SetPosition(position);

            if (presenceSceneProvider.Scene != null)
            {
                DetectOverlapsWithRegions();
            }
        }

        public void SetSize()
        {
            var size = presenceSceneProvider.Scene.RegionSize;
            interestArea.SetSize(size);
        }

        public IEnumerable<IRegion> GetSubscribedPublishers()
        {
            var regions = presenceSceneProvider.Scene.GetAllRegions();
            return regions.Cast<IRegion>().Where(region => region.HasSubscription(Entity.Id)).ToArray();
        }

        public void DetectOverlapsWithRegions()
        {
            if (Entity == null)
            {
                LogUtils.Log(MessageBuilder.Trace("Entity is null."));
                return;
            }

            var sceneRegions = presenceSceneProvider.Scene.GetAllRegions();
            foreach (var region in sceneRegions)
            {
                if (region == null)
                {
                    LogUtils.Log(MessageBuilder.Trace("Region is null."));
                    continue;
                }

                if (IsIntersect(region.PublisherArea, interestArea))
                {
                    if (region.HasSubscription(Entity.Id))
                    {
                        continue;
                    }

                    region.AddSubscription(Entity);
                }
                else
                {
                    if (region.HasSubscription(Entity.Id))
                    {
                        region.RemoveSubscription(Entity.Id);
                    }
                }
            }
        }

        private bool IsIntersect(Rectangle publisherArea, Rectangle interestArea)
        {
            return !Rectangle.Intersect(publisherArea, interestArea).Equals(Rectangle.Empty);
        }
    }
}