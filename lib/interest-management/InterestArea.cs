using System.Collections.Generic;
using System.Linq;

namespace InterestManagement
{
    public class InterestArea<TSceneObject> : IInterestArea<TSceneObject>
        where TSceneObject : ISceneObject
    {
        private readonly IScene<TSceneObject> scene;
        private readonly TSceneObject sceneObject;

        private readonly List<IRegion<TSceneObject>> regions;
        private readonly NearbySceneObjectsCollection<TSceneObject> nearbySceneObjects;

        public InterestArea(IScene<TSceneObject> scene, TSceneObject sceneObject)
        {
            this.scene = scene;
            this.sceneObject = sceneObject;

            regions = new List<IRegion<TSceneObject>>();
            nearbySceneObjects = new NearbySceneObjectsCollection<TSceneObject>();

            UpdateNearbyRegions();
            SubscribeToPositionChanged();
        }

        public void Dispose()
        {
            UnsubscribeFromPositionChanged();

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
            var visibleRegions =
                scene.MatrixRegion.GetRegions(sceneObject.Transform);
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
                        var subscribers = region.GetAllSubscribers();
                        nearbySceneObjects.Add(subscribers);
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
                            !region.IsOverlaps(
                                sceneObject.Transform.Position,
                                sceneObject.Transform.Size))
                    .ToArray();

            foreach (var region in invisibleRegions)
            {
                regions.Remove(region);

                region.Unsubscribe(sceneObject);

                if (region.SubscriberCount() != 0)
                {
                    var subscribers = region.GetAllSubscribers();
                    foreach (var subscriber in subscribers)
                    {
                        if (!IsOverlapsWithNearbyRegions(subscriber))
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
            if (!IsOverlapsWithNearbyRegions(sceneObject))
            {
                nearbySceneObjects.Remove(sceneObject);
            }
        }

        private bool IsOverlapsWithNearbyRegions(TSceneObject sceneObject)
        {
            return regions.Any(
                x => x.IsOverlaps(
                    sceneObject.Transform.Position,
                    sceneObject.Transform.Size));
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