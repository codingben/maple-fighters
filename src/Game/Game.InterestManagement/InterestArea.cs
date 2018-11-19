using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
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

                    region.Subscribe(sceneObject);

                    regions.Add(region);

                    if (region.HasSubscribers())
                    {
                        var subscribers = region.GetAllSubscribers()
                            .ExcludeSceneObject(sceneObject);

                        sceneObjects.Add(subscribers);
                    }

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
                            !region.Rectangle.Intersects(
                                sceneObject.Transform.Position,
                                sceneObject.Transform.Size))
                    .ToArray();

            foreach (var region in invisibleRegions)
            {
                region.Unsubscribe(sceneObject);

                regions.Remove(region);

                if (region.HasSubscribers())
                {
                    var subscribers = region.GetAllSubscribers()
                        .ExcludeSceneObject(sceneObject);

                    sceneObjects.Remove(subscribers);
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
            sceneObjects.Remove(sceneObject);
        }
    }
}