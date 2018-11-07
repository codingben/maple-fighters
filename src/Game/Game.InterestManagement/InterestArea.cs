using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public class InterestArea : IInterestArea
    {
        private readonly IScene scene;
        private readonly ISceneObject sceneObject;

        private readonly List<IRegion> nearbyRegions = new List<IRegion>();

        private readonly NearbySubscriberCollection nearbySubscriberCollection;

        public InterestArea(IScene scene, ISceneObject sceneObject)
        {
            this.scene = scene;
            this.sceneObject = sceneObject;

            nearbySubscriberCollection =
                new NearbySubscriberCollection(sceneObject);

            SubscribeToPositionChanged();
        }

        public void Dispose()
        {
            UnsubscribeFromPositionChanged();

            nearbyRegions?.Clear();
        }

        /// <inheritdoc />
        public INearbySubscriberCollection GetNearbySubscribers()
        {
            return nearbySubscriberCollection;
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
                scene.MatrixRegion.GetRegions(sceneObject.Transform)?.ToArray();
            if (visibleRegions != null)
            {
                foreach (var region in visibleRegions)
                {
                    if (!nearbyRegions.Contains(region))
                    {
                        nearbyRegions.Add(region);
                    }
                }

                if (visibleRegions.Length != 0)
                {
                    nearbySubscriberCollection.OnNearbyRegionsAdded(
                        visibleRegions);
                }
            }
        }

        private void UnsubscribeFromInvisibleRegions()
        {
            var invisibleRegions =
                nearbyRegions.Where(
                    x => !x.Rectangle.Intersects(
                             sceneObject.Transform.Position,
                             sceneObject.Transform.Size)).ToArray();

            foreach (var region in invisibleRegions)
            {
                nearbyRegions.Remove(region);
            }

            if (invisibleRegions.Length != 0)
            {
                nearbySubscriberCollection.OnNearbyRegionsRemoved(
                    invisibleRegions);
            }
        }
    }
}