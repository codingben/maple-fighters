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
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{subscriberArea.Entity.Id} already exists in a region."), LogMessageType.Error);
                return;
            }

            subscribersAreas.Add(subscriberArea.Entity.Id, subscriberArea);

            AddSubscribersForSubscriber(subscriberArea);
            AddSubscriberForSubscribers(subscriberArea);

            LogUtils.Log(MessageBuilder.Trace($"Added subscription id #{subscriberArea.Entity.Id}"));
        }

        public void RemoveSubscription(int subscriberId)
        {
            if (!subscribersAreas.ContainsKey(subscriberId))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{subscriberId} does not exists in a region."), LogMessageType.Error);
                return;
            }

            RemoveSubscribersForSubscriber(subscriberId);

            subscribersAreas.Remove(subscriberId);

            RemoveSubscriberForSubscribers(subscriberId);

            LogUtils.Log(MessageBuilder.Trace($"Removed subscription id #{subscriberId}"));
        }

        public void RemoveSubscriptionForAllSubscribers(int subscriberId)
        {
            if (!subscribersAreas.ContainsKey(subscriberId))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{subscriberId} does not exists in a region."), LogMessageType.Error);
                return;
            }

            RemoveAllSubscribersForSubscriber(subscriberId);

            subscribersAreas.Remove(subscriberId);

            RemoveSubscriberForAllSubscribers(subscriberId);
        }

        public bool HasSubscription(int subscriberId)
        {
            return subscribersAreas.ContainsKey(subscriberId);
        }

        public IEnumerable<InterestArea> GetAllSubscribersArea()
        {
            return subscribersAreas.Values;
        }

        /// <summary>
        /// Add all subscribers for a new subscriber.
        /// </summary>
        /// <param name="subscriberArea">A new subscriber</param>
        private void AddSubscribersForSubscriber(InterestArea subscriberArea)
        {
            var subscribers = subscribersAreas.Values.Where(subscriber => subscriber.Entity.Id != subscriberArea.Entity.Id).ToArray();
            subscriberArea?.SubscribersAdded?.Invoke(subscribers); 
        }

        /// <summary>
        /// Remove subscribers for the subscriber that left this region.
        /// </summary>
        /// <param name="subscriberId">A removed subscriber id</param>
        private void RemoveSubscribersForSubscriber(int subscriberId)
        {
            var subscribers = subscribersAreas.Values.Where(subscriber => subscriber.Entity.Id != subscriberId).ToArray();
            var interestArea = subscribersAreas[subscriberId];

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

        /// <summary>
        /// Add a new subscriber for all other subscribers.
        /// </summary>
        /// <param name="subscriberArea">A new subscriber</param>
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

        /// <summary>
        /// Remove the subscriber that left from this region for other subscribers.
        /// </summary>
        /// <param name="subscriberId">A removed subscriber id</param>
        private void RemoveSubscriberForSubscribers(int subscriberId)
        {
            foreach (var subscriber in subscribersAreas.Values)
            {
                if (!subscriber.GetPublishers().Any(publisher => publisher.HasSubscription(subscriberId)))
                {
                    subscriber.SubscriberRemoved?.Invoke(subscriberId);
                }
            }
        }

        /// <summary>
        /// Remove the subscriber that left from this region for all subscribers.
        /// </summary>
        /// <param name="subscriberId">A removed subscriber id</param>
        private void RemoveSubscriberForAllSubscribers(int subscriberId)
        {
            foreach (var subscriber in subscribersAreas.Values)
            {
                subscriber.SubscriberRemoved?.Invoke(subscriberId);
            }
        }

        /// <summary>
        /// Remove subscribers for the subscriber that left from this region.
        /// </summary>
        /// <param name="subscriberId">A removed subscriber id</param>
        private void RemoveAllSubscribersForSubscriber(int subscriberId)
        {
            var subscribers = subscribersAreas.Keys.Where(id => id != subscriberId).ToArray();

            if (subscribersAreas[subscriberId] == null)
            {
                return;
            }

            subscribersAreas[subscriberId]?.SubscribersRemoved?.Invoke(subscribers);
        }
    }
}