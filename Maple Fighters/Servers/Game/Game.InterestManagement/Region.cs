using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Region : IRegion
    {
        public Rectangle PublisherArea { get; }

        private readonly Dictionary<int, InterestArea> subscribersAreas = new Dictionary<int, InterestArea>();

        public Region(Rectangle rectangle)
        {
            PublisherArea = rectangle;
        }

        public void AddSubscription(InterestArea subscriberArea)
        {
            if (subscribersAreas.ContainsKey(subscriberArea.Entity.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{subscriberArea.Entity.Id} already exists in a region."), LogMessageType.Error);
                return;
            }

            LogUtils.Log(MessageBuilder.Trace($"Added subscription id #{subscriberArea.Entity.Id}"));

            subscribersAreas.Add(subscriberArea.Entity.Id, subscriberArea);

            // Show all exists entities for a new game object.
            AddSubscribersForSubscriber(subscriberArea);

            // Show a new game object for all exists entities.
            AddSubscriberForSubscribers(subscriberArea);
        }

        public void RemoveSubscription(int subscriberId)
        {
            if (!subscribersAreas.ContainsKey(subscriberId))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{subscriberId} does not exists in a region."), LogMessageType.Error);
                return;
            }

            LogUtils.Log(MessageBuilder.Trace($"Removed subscription id #{subscriberId}"));

            // Hide game objects for the one that left this region.
            RemoveSubscribersForSubscriber(subscriberId);

            // Remove him from region's list.
            subscribersAreas.Remove(subscriberId);

            // Hide the one who left from this region for other game objects.
            RemoveSubscriberForSubscribers(subscriberId);
        }

        public void RemoveSubscriptionForOtherOnly(int subscriberId)
        {
            if (!subscribersAreas.ContainsKey(subscriberId))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{subscriberId} does not exists in a region."), LogMessageType.Error);
                return;
            }

            RemoveSubscribersForSubscriberOnly(subscriberId);

            // Remove him from region's list.
            subscribersAreas.Remove(subscriberId);

            // Hide the one who left from this region for other game objects.
            RemoveSubscriberForOtherOnly(subscriberId);
        }

        public bool HasSubscription(int subscriberId)
        {
            return subscribersAreas.ContainsKey(subscriberId);
        }

        public IEnumerable<InterestArea> GetAllSubscribersArea()
        {
            return subscribersAreas.Values;
        }

        private void AddSubscribersForSubscriber(InterestArea subscriberArea)
        {
            var subscribers = subscribersAreas.Values.Where(subscriber => subscriber.Entity.Id != subscriberArea.Entity.Id).ToArray();
            subscriberArea?.SubscribersAdded?.Invoke(subscribers); 
        }

        private void RemoveSubscribersForSubscriber(int removeSubscriberId)
        {
            var subscribers = subscribersAreas.Values.Where(subscriber => subscriber.Entity.Id != removeSubscriberId).ToArray();
            var interestArea = subscribersAreas[removeSubscriberId];

            var subscribersForRemoveList = new List<int>();

            foreach (var subscriber in subscribers)
            {
                if (interestArea.GetPublishers().Any(publisher => !publisher.HasSubscription(subscriber.Entity.Id)))
                {
                    subscribersForRemoveList.Add(subscriber.Entity.Id);
                }
            }

            if (subscribersForRemoveList.Count > 0)
            {
                interestArea.SubscribersRemoved?.Invoke(subscribersForRemoveList.ToArray());
            }
        }

        private void AddSubscriberForSubscribers(InterestArea subscriberArea)
        {
            foreach (var subscriber in subscribersAreas)
            {
                if (subscriber.Value.Entity.Id == subscriberArea.Entity.Id)
                {
                    continue;
                }

                subscriber.Value.SubscriberAdded?.Invoke(subscriberArea);
            }
        }

        private void RemoveSubscriberForSubscribers(int removeSubscriberId)
        {
            foreach (var subscriber in subscribersAreas.Values)
            {
                if (!subscriber.GetPublishers().Any(publisher => publisher.HasSubscription(removeSubscriberId)))
                {
                    subscriber.SubscriberRemoved?.Invoke(removeSubscriberId);
                }
            }
        }

        private void RemoveSubscriberForOtherOnly(int removeSubscriberId)
        {
            foreach (var subscriber in subscribersAreas.Values)
            {
                subscriber.SubscriberRemoved?.Invoke(removeSubscriberId);
            }
        }

        private void RemoveSubscribersForSubscriberOnly(int removeSubscriberId)
        {
            var subscribers = subscribersAreas.Keys.Where(subscriberId => subscriberId != removeSubscriberId).ToArray();

            if (subscribersAreas[removeSubscriberId] == null)
            {
                return;
            }

            subscribersAreas[removeSubscriberId]?.SubscribersRemoved?.Invoke(subscribers);
        }
    }
}