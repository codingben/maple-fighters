using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    public class InterestArea : Component<ISceneObject>
    {
        public Action<InterestArea> SubscriberAdded;
        public Action<int> SubscriberRemoved;
        public Action<InterestArea[]> SubscribersAdded;
        public Action<int[]> SubscribersRemoved;

        public readonly Action DetectOverlapsWithRegionsAction;

        private Rectangle interestArea;

        public InterestArea(Vector2 position, Vector2 areaSize)
        {
            interestArea = new Rectangle(position, areaSize);

            DetectOverlapsWithRegionsAction = DetectOverlapsWithRegions;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            SubscribeToPositionChangesNotifier();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnsubscribeFromPositionChangesNotifier();
        }

        private void SubscribeToPositionChangesNotifier()
        {
            var transform = Entity.Container.GetComponent<Transform>().AssertNotNull();
            transform.PositionChanged += SetPosition;
        }

        private void UnsubscribeFromPositionChangesNotifier()
        {
            var transform = Entity.Container.GetComponent<Transform>().AssertNotNull();
            transform.PositionChanged -= SetPosition;
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

        private void DetectOverlapsWithRegions()
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

                    region.AddSubscription(this);
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