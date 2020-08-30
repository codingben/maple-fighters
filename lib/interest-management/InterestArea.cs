using System.Collections.Generic;
using System.Linq;

namespace InterestManagement
{
    public class InterestArea<TSceneObject> : IInterestArea<TSceneObject>
        where TSceneObject : ISceneObject
    {
        private readonly TSceneObject sceneObject;
        private readonly List<IRegion<TSceneObject>> regions;
        private readonly NearbySceneObjectsCollection<TSceneObject> nearbySceneObjects;

        private IMatrixRegion<TSceneObject> matrixRegion;

        public InterestArea(TSceneObject sceneObject)
        {
            this.sceneObject = sceneObject;

            regions = new List<IRegion<TSceneObject>>();
            nearbySceneObjects = new NearbySceneObjectsCollection<TSceneObject>();
        }

        public void SetMatrixRegion(IMatrixRegion<TSceneObject> matrixRegion)
        {
            this.matrixRegion = matrixRegion;

            UpdateNearbyRegions();
            SubscribeToPositionChanged();
        }

        public void Dispose()
        {
            UnsubscribeFromPositionChanged();

            foreach (var region in regions)
            {
                region.Unsubscribe(sceneObject);
            }

            regions?.Clear();
            nearbySceneObjects?.Clear();
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
            var visibleRegions = matrixRegion.GetRegions(sceneObject.Transform);
            if (visibleRegions != null)
            {
                foreach (var region in visibleRegions)
                {
                    if (regions.Contains(region))
                    {
                        continue;
                    }

                    regions.Add(region);

                    if (region.SubscriberCount() != 0)
                    {
                        nearbySceneObjects.Add(region.GetAllSubscribers());
                    }

                    region.Subscribe(sceneObject);

                    SubscribeToRegionEvents(region);
                }
            }
        }

        private void UnsubscribeFromInvisibleRegions()
        {
            var invisibleRegions =
                regions
                    .Where(
                        region =>
                            !region.IsOverlaps(sceneObject.Transform))
                    .ToArray();

            foreach (var region in invisibleRegions)
            {
                regions.Remove(region);

                region.Unsubscribe(sceneObject);

                if (region.SubscriberCount() != 0)
                {
                    foreach (var subscriber in region.GetAllSubscribers())
                    {
                        var transform = sceneObject.Transform;

                        if (IsOverlapsWithNearbyRegions(transform) == false)
                        {
                            nearbySceneObjects.Remove(subscriber);
                        }
                    }
                }

                UnsubscribeFromRegionEvents(region);
            }
        }

        private void SubscribeToRegionEvents(IRegion<TSceneObject> region)
        {
            region.SubscriberAdded += OnSubscriberAdded;
            region.SubscriberRemoved += OnSubscriberRemoved;
        }

        private void UnsubscribeFromRegionEvents(IRegion<TSceneObject> region)
        {
            region.SubscriberAdded -= OnSubscriberAdded;
            region.SubscriberRemoved -= OnSubscriberRemoved;
        }

        private void OnSubscriberAdded(TSceneObject sceneObject)
        {
            nearbySceneObjects.Add(sceneObject);
        }

        private void OnSubscriberRemoved(TSceneObject sceneObject)
        {
            var transform = sceneObject.Transform;

            if (IsOverlapsWithNearbyRegions(transform) == false)
            {
                nearbySceneObjects.Remove(sceneObject);
            }
        }

        private bool IsOverlapsWithNearbyRegions(ITransform transform)
        {
            return regions.Any(region => region.IsOverlaps(transform));
        }

        public IEnumerable<IRegion<TSceneObject>> GetRegions()
        {
            return regions;
        }

        public IEnumerable<TSceneObject> GetNearbySceneObjects()
        {
            return nearbySceneObjects.GetSceneObjects();
        }

        public INearbySceneObjectsEvents<TSceneObject> GetNearbySceneObjectsEvents()
        {
            return nearbySceneObjects;
        }
    }
}