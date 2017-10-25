using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using ComponentModel.Common;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public class InterestArea : Component<ISceneObject>, IInterestArea
    {
        public event Action<ISceneObject> SubscriberAdded;
        public event Action<int> SubscriberRemoved;
        public event Action<ISceneObject[]> SubscribersAdded;
        public event Action<int[]> SubscribersRemoved;

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

        private Rectangle interestArea;
        private readonly Dictionary<int, ISceneObject> interestedSceneObjects = new Dictionary<int, ISceneObject>();

        public InterestArea(Vector2 position, Vector2 areaSize)
        {
            interestArea = new Rectangle(position, areaSize);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            SubscribeToPositionChangesNotifier();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            SubscriberAdded = null;
            SubscriberRemoved = null;
            SubscribersAdded = null;
            SubscribersRemoved = null;

            interestedSceneObjects.Clear();
        }

        private void SubscribeToPositionChangesNotifier()
        {
            var transform = Entity.Container.GetComponent<ITransform>().AssertNotNull();
            transform.PositionChanged += SetPosition;
        }

        private void SetPosition(Vector2 position)
        {
            interestArea.SetPosition(position);

            if (Entity.Scene != null)
            {
                DetectOverlapsWithRegions();
            }
        }

        public void SetSize()
        {
            var size = Entity.Scene.RegionSize;
            interestArea.SetSize(size);
        }

        public IEnumerable<IRegion> GetSubscribedPublishers()
        {
            var regions = Entity.Scene.GetAllRegions();
            return regions.Cast<IRegion>().Where(region => region.HasSubscription(Entity.Id)).ToArray();
        }

        public void DetectOverlapsWithRegions()
        {
            if (Entity == null)
            {
                LogUtils.Log(MessageBuilder.Trace("Entity is null."));
                return;
            }

            var sceneRegions = Entity.Scene.GetAllRegions();

            foreach (var region in sceneRegions)
            {
                if (region == null)
                {
                    LogUtils.Log(MessageBuilder.Trace("Region is null."));
                    continue;
                }

                if (!Rectangle.Intersect(region.PublisherArea, interestArea).Equals(Rectangle.EMPTY))
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
    }
}