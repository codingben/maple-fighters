using System.Collections.Generic;
using System.Linq;

namespace InterestManagement
{
    public class InterestArea<TObject> : IInterestArea<TObject>
        where TObject : ISceneObject
    {
        public INearbySceneObjectsEvents<TObject> NearbySceneObjectsEvents =>
            sceneObjects;

        private readonly IScene<TObject> scene;
        private readonly TObject sceneObject;
        private readonly List<IRegion<TObject>> regions;
        private readonly NearbySceneObjectsCollection<TObject> sceneObjects;

        public InterestArea(IScene<TObject> scene, TObject sceneObject)
        {
            this.scene = scene;
            this.sceneObject = sceneObject;

            regions = new List<IRegion<TObject>>();
            sceneObjects = new NearbySceneObjectsCollection<TObject>();

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

        private void SubscribeToRegionEvents(IRegion<TObject> region)
        {
            region.SubscriberAdded += OnSubscriberAdded;
            region.SubscriberRemoved += OnSubscriberRemoved;
        }

        private void UnsubscribeFromRegionEvents(IRegion<TObject> region)
        {
            region.SubscriberAdded -= OnSubscriberAdded;
            region.SubscriberRemoved -= OnSubscriberRemoved;
        }

        private void OnSubscriberAdded(TObject sceneObject)
        {
            sceneObjects.Add(sceneObject);
        }

        private void OnSubscriberRemoved(TObject sceneObject)
        {
            if (!IsOverlapsWithNearbyRegions(sceneObject))
            {
                sceneObjects.Remove(sceneObject);
            }
        }

        private bool IsOverlapsWithNearbyRegions(TObject sceneObject)
        {
            return regions.Any(
                x => x.IsOverlaps(
                    sceneObject.Transform.Position,
                    sceneObject.Transform.Size));
        }
    }
}