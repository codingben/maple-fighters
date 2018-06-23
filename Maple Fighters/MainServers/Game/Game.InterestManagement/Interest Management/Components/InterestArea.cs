using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using ComponentModel.Common;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;

namespace InterestManagement.Components
{
    public class InterestArea : Component<ISceneObject>, IInterestArea
    {
        private Rectangle interestArea;
        private IPresenceSceneProvider presenceSceneProvider;

        public InterestArea(Vector2 position, Vector2 areaSize)
        {
            interestArea = new Rectangle(position, areaSize);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            Entity.Components.AddComponent(new NearbySubscribersCollection());
            presenceSceneProvider = Entity.Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();

            SubscribeToPositionChanged();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnubscribeFromPositionChanged();
        }

        private void SubscribeToPositionChanged()
        {
            var positionChangesNotifier = Entity.Components.GetComponent<IPositionChangesNotifier>().AssertNotNull();
            positionChangesNotifier.PositionChanged += OnPositionChanged;
        }

        private void UnubscribeFromPositionChanged()
        {
            var positionChangesNotifier = Entity.Components.GetComponent<IPositionChangesNotifier>().AssertNotNull();
            positionChangesNotifier.PositionChanged -= OnPositionChanged;
        }

        private void OnPositionChanged(Vector2 position)
        {
            interestArea.SetPosition(position);

            if (Entity != null && presenceSceneProvider.GetScene() != null)
            {
                DetectOverlapsWithRegions();
            }
        }

        public void SetSize()
        {
            var scene = presenceSceneProvider.GetScene();
            interestArea.SetSize(scene.RegionSize);
        }

        public void DetectOverlapsWithRegions()
        {
            var sceneRegions = presenceSceneProvider.GetScene().GetAllRegions();
            foreach (var region in sceneRegions)
            {
                if (IsIntersect(region.PublisherArea, interestArea))
                {
                    if (region.HasSubscription(Entity))
                    {
                        continue;
                    }

                    region.AddSubscription(Entity);
                }
                else
                {
                    if (region.HasSubscription(Entity))
                    {
                        region.RemoveSubscription(Entity);
                    }
                }
            }

            bool IsIntersect(Rectangle publisherArea, Rectangle interestArea) => !Rectangle.Intersect(publisherArea, interestArea).Equals(Rectangle.Empty);
        }

        public IEnumerable<IRegion> GetSubscribedPublishers()
        {
            var regions = presenceSceneProvider.GetScene().GetAllRegions();
            return regions.Cast<IRegion>().Where(region => region.HasSubscription(Entity)).ToArray();
        }
    }
}