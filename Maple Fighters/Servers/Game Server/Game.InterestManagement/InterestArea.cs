using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public class InterestArea : GameObjectComponent, IInterestArea
    {
        private Rectangle interestArea;

        public InterestArea(Vector2 areaSize)
        {
            interestArea = new Rectangle(Vector2.Zero, areaSize);
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
            var transform = GameObject.Entity.GetComponent<Transform>()?.AssertNotNull();
            transform.PositionChanged += SetPosition;
        }

        private void UnsubscribeFromPositionChangesNotifier()
        {
            var transform = GameObject.Entity.GetComponent<Transform>().AssertNotNull();
            transform.PositionChanged -= SetPosition;
        }

        private void SetPosition(Vector2 position)
        {
            interestArea.SetPosition(position);

            DetectOverlapsWithRegions();
        }

        public IEnumerable<IRegion> GetPublishers()
        {
            var regions = GameObject.Scene.GetAllRegions();
            return regions.Cast<IRegion>().Where(region => region.HasSubscription(GameObject.Id)).ToArray();
        }

        public IEnumerable<IRegion> GetPublishersExceptMyGameObject()
        {
            var regions = GameObject.Scene.GetAllRegions();
            var publishers = regions.Cast<IRegion>().Where(region => region.HasSubscription(GameObject.Id));

            var publishersExceptMyGameObject = publishers as IRegion[] ?? publishers.ToArray();
            foreach (var publisher in publishersExceptMyGameObject)
            {
                if (publisher.HasSubscription(GameObject.Id))
                {
                    publisher.RemoveSubscription(GameObject);
                }
            }

            return publishersExceptMyGameObject;
        }

        private void DetectOverlapsWithRegions()
        {
            var sceneRegions = GameObject.Scene.GetAllRegions();

            foreach (var region in sceneRegions)
            {
                if (!Rectangle.Intersect(region.Area, interestArea).Equals(Rectangle.EMPTY))
                {
                    if (region.HasSubscription(GameObject.Id))
                    {
                        continue;
                    }

                    region.AddSubscription(GameObject);
                }
                else
                {
                    if (region.HasSubscription(GameObject.Id))
                    {
                        region.RemoveSubscription(GameObject);
                    }
                }
            }
        }
    }
}