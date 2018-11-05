using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public class NearbySceneObjectsCollection : INearbySceneObjectsCollection
    {
        /// <inheritdoc />
        public event Action<ISceneObject[]> NearbySceneObjectsAdded;

        /// <inheritdoc />
        public event Action<ISceneObject[]> NearbySceneObjectsRemoved;

        private readonly IInterestArea interestArea;
        private readonly ISceneObject sceneObject;

        private readonly HashSet<ISceneObject> sceneObjects = new HashSet<ISceneObject>();

        public NearbySceneObjectsCollection(
            IInterestArea interestArea,
            ISceneObject sceneObject)
        {
            this.interestArea = interestArea;
            this.sceneObject = sceneObject;

            SubscribeToInterestAreaEvents();
        }

        public void Dispose()
        {
            UnsubscribeFromInterestAreaEvents();

            sceneObjects?.Clear();
        }

        private void SubscribeToInterestAreaEvents()
        {
            interestArea.NearbyRegionsAdded += OnNearbyRegionsAdded;
            interestArea.NearbyRegionsRemoved += OnNearbyRegionsRemoved;
        }

        private void UnsubscribeFromInterestAreaEvents()
        {
            interestArea.NearbyRegionsAdded -= OnNearbyRegionsAdded;
            interestArea.NearbyRegionsRemoved -= OnNearbyRegionsRemoved;
        }

        private void OnNearbyRegionsAdded(IRegion[] regions)
        {
            foreach (var region in regions)
            {
                var subscribers = region.GetAllSubscribers()
                    .Where(x => x.Id != sceneObject.Id && sceneObjects.Add(x))
                    .ToArray();

                NearbySceneObjectsAdded?.Invoke(subscribers);

                SubscribeToRegionEvents(region);
            }

            void SubscribeToRegionEvents(IRegion region)
            {
                region.SubscriberAdded += OnSubscriberAdded;
                region.SubscriberRemoved += OnSubscriberRemoved;
            }
        }

        private void OnNearbyRegionsRemoved(IRegion[] regions)
        {
            foreach (var region in regions)
            {
                var subscribers = region.GetAllSubscribers()
                    .Where(x => x.Id != sceneObject.Id && sceneObjects.Remove(x))
                    .ToArray();

                NearbySceneObjectsRemoved?.Invoke(subscribers);

                UnsubscribeFromRegionEvents(region);
            }

            void UnsubscribeFromRegionEvents(IRegion region)
            {
                region.SubscriberAdded -= OnSubscriberAdded;
                region.SubscriberRemoved -= OnSubscriberRemoved;
            }
        }

        private void OnSubscriberAdded(ISceneObject subscriber)
        {
            if (subscriber.Id != sceneObject.Id)
            {
                NearbySceneObjectsAdded?.Invoke(new[] { subscriber });
            }
        }

        private void OnSubscriberRemoved(ISceneObject subscriber)
        {
            if (subscriber.Id != sceneObject.Id)
            {
                NearbySceneObjectsRemoved?.Invoke(new[] { subscriber });
            }
        }
    }
}