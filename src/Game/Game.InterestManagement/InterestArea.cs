using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public class InterestArea : IInterestArea
    {
        private readonly IScene scene;
        private readonly ISceneObject sceneObject;
        private readonly List<IRegion> nearbyRegions = new List<IRegion>();
        private readonly NearbySceneObjectsCollection nearbySceneObjectsCollection;

        public InterestArea(IScene scene, ISceneObject sceneObject)
        {
            this.scene = scene;
            this.sceneObject = sceneObject;

            nearbySceneObjectsCollection =
                new NearbySceneObjectsCollection(excludedId: sceneObject.Id);

            SubscribeToPositionChanged();
        }

        public void Dispose()
        {
            UnsubscribeFromPositionChanged();

            nearbyRegions?.Clear();
        }

        /// <inheritdoc />
        public INearbySceneObjectsCollection GetNearbySceneObjects()
        {
            return nearbySceneObjectsCollection;
        }

        private void SubscribeToPositionChanged()
        {
            sceneObject.Transform.PositionChanged += UpdateNearbyRegions;
        }

        private void UnsubscribeFromPositionChanged()
        {
            sceneObject.Transform.PositionChanged -= UpdateNearbyRegions;
        }

        private void UpdateNearbyRegions()
        {
            SubscribeToVisibleRegions();
            UnsubscribeFromInvisibleRegions();
        }

        private void SubscribeToVisibleRegions()
        {
            var visibleRegions =
                scene.MatrixRegion.GetRegions(sceneObject.Transform);
            if (visibleRegions != null)
            {
                foreach (var region in visibleRegions)
                {
                    if (nearbyRegions.Contains(region))
                    {
                        continue;
                    }

                    nearbyRegions.Add(region);
                    nearbySceneObjectsCollection.Add(region.GetAllSubscribers());

                    SubscribeToRegionEvents(region);
                }
            }
        }

        private void UnsubscribeFromInvisibleRegions()
        {
            var invisibleRegions =
                nearbyRegions.Where(
                    x => !x.Rectangle.Intersects(
                             sceneObject.Transform.Position, 
                             sceneObject.Transform.Size));

            foreach (var region in invisibleRegions)
            {
                nearbyRegions.Remove(region);
                nearbySceneObjectsCollection.Remove(region.GetAllSubscribers());

                UnsubscribeFromRegionEvents(region);
            }
        }

        private void SubscribeToRegionEvents(IRegion region)
        {
            region.SubscriberAdded += OnSubscriberAdded;
            region.SubscriberRemoved += OnSubscriberRemoved;
        }

        private void UnsubscribeFromRegionEvents(IRegion region)
        {
            region.SubscriberAdded -= OnSubscriberAdded;
            region.SubscriberRemoved -= OnSubscriberRemoved;
        }

        private void OnSubscriberAdded(ISceneObject sceneObject)
        {
            nearbySceneObjectsCollection.Add(sceneObject);
        }

        private void OnSubscriberRemoved(ISceneObject sceneObject)
        {
            nearbySceneObjectsCollection.Remove(sceneObject);
        }
    }
}