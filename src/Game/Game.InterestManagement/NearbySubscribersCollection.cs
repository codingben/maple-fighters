using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public class NearbySubscriberCollection : INearbySubscriberCollection
    {
        /// <inheritdoc />
        public event Action<ISceneObject> SubscriberAdded;

        /// <inheritdoc />
        public event Action<ISceneObject> SubscriberRemoved;

        /// <inheritdoc />
        public event Action<IEnumerable<ISceneObject>> SubscribersAdded;

        /// <inheritdoc />
        public event Action<IEnumerable<ISceneObject>> SubscribersRemoved;

        private readonly ISceneObject excludeSceneObject;
        private readonly HashSet<ISceneObject> nearbySubscribers = new HashSet<ISceneObject>();

        public NearbySubscriberCollection(ISceneObject excludeSceneObject)
        {
            this.excludeSceneObject = excludeSceneObject;
        }

        public void Dispose()
        {
            nearbySubscribers?.Clear();
        }

        public void OnNearbyRegionsAdded(IEnumerable<IRegion> regions)
        {
            foreach (var region in regions)
            {
                var visibleSubscribers = 
                    region.GetAllSubscribers().Where(
                        x => x.Id != excludeSceneObject.Id
                             && nearbySubscribers.Add(x)).ToArray();

                if (visibleSubscribers.Length != 0)
                {
                    SubscribersAdded?.Invoke(visibleSubscribers);
                }

                SubscribeToRegionEvents(region);
            }

            void SubscribeToRegionEvents(IRegion region)
            {
                region.SubscriberAdded += OnSubscriberAdded;
                region.SubscriberRemoved += OnSubscriberRemoved;
            }
        }

        public void OnNearbyRegionsRemoved(IEnumerable<IRegion> regions)
        {
            foreach (var region in regions)
            {
                var invisibleSubscribers = 
                    region.GetAllSubscribers().Where(
                        x => x.Id != excludeSceneObject.Id 
                             && nearbySubscribers.Remove(x)).ToArray();

                if (invisibleSubscribers.Length != 0)
                {
                    SubscribersRemoved?.Invoke(invisibleSubscribers);
                }

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
            if (subscriber.Id != excludeSceneObject.Id)
            {
                SubscriberAdded?.Invoke(subscriber);
            }
        }

        private void OnSubscriberRemoved(ISceneObject subscriber)
        {
            if (subscriber.Id != excludeSceneObject.Id)
            {
                SubscriberRemoved?.Invoke(subscriber);
            }
        }
    }
}