using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public class InterestArea : IInterestArea
    {
        public INearbySceneObjectsEvents NearbySceneObjectsEvents =>
            nearbySceneObjectsCollection;

        private readonly IScene scene;
        private readonly ISceneObject sceneObject;
        private readonly List<IRegion> nearbyRegions;
        private readonly NearbySceneObjectsCollection nearbySceneObjectsCollection;

        public InterestArea(IScene scene, ISceneObject sceneObject)
        {
            this.scene = scene;
            this.sceneObject = sceneObject;

            nearbySceneObjectsCollection = new NearbySceneObjectsCollection();
            nearbyRegions = new List<IRegion>();

            SubscribeToPositionChanged();
        }

        public void Dispose()
        {
            UnsubscribeFromPositionChanged();

            nearbyRegions?.Clear();
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

                    region.Subscribe(sceneObject);

                    nearbyRegions.Add(region);

                    if (region.HasSubscribers())
                    {
                        var subscribers = region.GetAllSubscribers()
                            .ExcludeSceneObject(sceneObject);

                        nearbySceneObjectsCollection.Add(subscribers);
                    }

                    SubscribeToRegionEvents(region);
                }
            }
        }

        private void UnsubscribeFromInvisibleRegions()
        {
            var invisibleRegions =
                nearbyRegions
                    .Where(
                        region => 
                            !region.Rectangle.Intersects(
                                sceneObject.Transform.Position,
                                sceneObject.Transform.Size))
                    .ToArray();

            foreach (var region in invisibleRegions)
            {
                region.Unsubscribe(sceneObject);

                nearbyRegions.Remove(region);

                if (region.HasSubscribers())
                {
                    var subscribers = region.GetAllSubscribers()
                        .ExcludeSceneObject(sceneObject);

                    nearbySceneObjectsCollection.Remove(subscribers);
                }

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