using System.Collections.Generic;
using System.Linq;

namespace InterestManagement
{
    public class InterestArea<TSceneObject> : IInterestArea<TSceneObject>
        where TSceneObject : ISceneObject
    {
        private readonly ILogger log;
        private readonly TSceneObject sceneObject;
        private readonly List<IRegion<TSceneObject>> regions;
        private readonly NearbySceneObjectsCollection<TSceneObject> nearbySceneObjects;

        private IMatrixRegion<TSceneObject> matrixRegion;

        public InterestArea(TSceneObject sceneObject, ILogger log = null)
        {
            this.log = log;
            this.sceneObject = sceneObject;

            regions = new List<IRegion<TSceneObject>>();
            nearbySceneObjects = new NearbySceneObjectsCollection<TSceneObject>(log);
        }

        public void SetMatrixRegion(IMatrixRegion<TSceneObject> matrixRegion)
        {
            log?.Info($"Set matrix region for scene object #{sceneObject.Id}");

            this.matrixRegion = matrixRegion;

            UpdateNearbyRegions();
            SubscribeToPositionChanged();
        }

        public void Dispose()
        {
            log?.Info($"Remove all regions from scene object #{sceneObject.Id}");

            UnsubscribeFromPositionChanged();
            UnsubscribeFromAllRegions();

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

        private void UnsubscribeFromAllRegions()
        {
            foreach (var region in regions)
            {
                region?.Unsubscribe(sceneObject);
            }
        }

        private void UpdateNearbyRegions()
        {
            // NOTE: Scene object size should be exactly same as region size,
            // if not then it'll add scene object and remove it.
            SubscribeToVisibleRegions();
            UnsubscribeFromInvisibleRegions();
        }

        private void SubscribeToVisibleRegions()
        {
            var visibleRegions =
                matrixRegion.GetRegions(sceneObject.Transform);
            if (visibleRegions != null)
            {
                foreach (var region in visibleRegions)
                {
                    if (regions.Contains(region))
                    {
                        continue;
                    }

                    log?.Info($"Add region for scene object #{sceneObject.Id}");

                    regions.Add(region);

                    AddNearbySceneObjects(region);

                    region.Subscribe(sceneObject);

                    SubscribeToRegionEvents(region);
                }
            }
        }

        private void UnsubscribeFromInvisibleRegions()
        {
            var invisibleRegions =
                regions.Where(x => !x.IsOverlaps(sceneObject.Transform))?.ToArray();
            if (invisibleRegions != null)
            {
                foreach (var region in invisibleRegions)
                {
                    log?.Info($"Remove region from scene object #{sceneObject.Id}");

                    regions.Remove(region);
                    region.Unsubscribe(sceneObject);

                    RemoveNearbySceneObjects(region);
                    UnsubscribeFromRegionEvents(region);
                }
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

            if (IsOverlapsWithNearbyRegions(transform))
            {
                return;
            }

            nearbySceneObjects.Remove(sceneObject);
        }

        private void AddNearbySceneObjects(IRegion<TSceneObject> region)
        {
            if (region.SubscriberCount() == 0)
            {
                return;
            }

            nearbySceneObjects.Add(region.GetAllSubscribers());
        }

        private void RemoveNearbySceneObjects(IRegion<TSceneObject> region)
        {
            if (region.SubscriberCount() == 0)
            {
                return;
            }

            foreach (var subscriber in region.GetAllSubscribers())
            {
                var transform = subscriber.Transform;

                if (IsOverlapsWithNearbyRegions(transform))
                {
                    continue;
                }

                nearbySceneObjects.Remove(subscriber);
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