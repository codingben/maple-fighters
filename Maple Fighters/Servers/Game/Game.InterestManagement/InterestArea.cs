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

        public void InvokeSubscriberAdded(ISceneObject sceneObject) => SubscriberAdded?.Invoke(sceneObject);
        public void InvokeSubscriberRemoved(int sceneObjectId) => SubscriberRemoved?.Invoke(sceneObjectId);
        public void InvokeSubscribersAdded(ISceneObject[] sceneObjects) => SubscribersAdded?.Invoke(sceneObjects);
        public void InvokeSubscribersRemoved(int[] sceneObjectIds) => SubscribersRemoved?.Invoke(sceneObjectIds);

        private Rectangle interestArea;

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

        public IEnumerable<IRegion> GetPublishers()
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