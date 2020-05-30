using System.Collections.Generic;
using System.Linq;

namespace InterestManagement
{
    public class InterestArea<TSceneObject> : IInterestArea<TSceneObject>
        where TSceneObject : ISceneObject
    {
        public INearbySceneObjectsEvents<TSceneObject> NearbySceneObjectsEvents =>
            sceneObjects;

        private readonly IScene<TSceneObject> scene;
        private readonly TSceneObject sceneObject;
        private readonly List<IRegion<TSceneObject>> regions;
        private readonly NearbySceneObjectsCollection<TSceneObject> sceneObjects;

        public InterestArea(IScene<TSceneObject> scene, TSceneObject sceneObject)
        {
            this.scene = scene;
            this.sceneObject = sceneObject;

            regions = new List<IRegion<TSceneObject>>();
            sceneObjects = new NearbySceneObjectsCollection<TSceneObject>();

            UpdateNearbyRegions();

            SubscribeToPositionChanged();
        }

        public void Dispose()
        {
            UnsubscribeFromPositionChanged();

            regions?.Clear();
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
                        sceneObjects.Add(subscribers);
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
                            sceneObjects.Remove(subscriber);
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
            sceneObjects.Add(sceneObject);
        }

        private void OnSubscriberRemoved(TSceneObject sceneObject)
        {
            if (!IsOverlapsWithNearbyRegions(sceneObject))
            {
                sceneObjects.Remove(sceneObject);
            }
        }

        private bool IsOverlapsWithNearbyRegions(TSceneObject sceneObject)
        {
            return regions.Any(
                x => x.IsOverlaps(
                    sceneObject.Transform.Position,
                    sceneObject.Transform.Size));
        }
    }
}