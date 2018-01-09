using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using MathematicsHelper;
using Config = JsonConfig.Config;

namespace Game.InterestManagement
{
    internal sealed class Region : IRegion
    {
        public Rectangle PublisherArea { get; }

        private readonly Dictionary<int, ISceneObject> sceneObjects = new Dictionary<int, ISceneObject>();

        public Region(Rectangle rectangle)
        {
            PublisherArea = rectangle;
        }

        public void AddSubscription(ISceneObject sceneObject)
        {
            if (sceneObjects.ContainsKey(sceneObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObject.Id} already exists in a region."), LogMessageType.Error);
                return;
            }

            sceneObjects.Add(sceneObject.Id, sceneObject);

            if (Config.Global.Log.InterestManagement)
            {
                LogUtils.Log(MessageBuilder.Trace($"Added subscription id #{sceneObject.Id}"));
            }

            AddSubscribersForSubscriber(sceneObject);
            AddSubscriberForSubscribers(sceneObject);
        }

        public void RemoveSubscription(int sceneObjectId)
        {
            if (!sceneObjects.ContainsKey(sceneObjectId))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObjectId} does not exists in a region."), LogMessageType.Error);
                return;
            }

            RemoveSubscribersForSubscriber(sceneObjects[sceneObjectId]);

            sceneObjects.Remove(sceneObjectId);

            if (Config.Global.Log.InterestManagement)
            {
                LogUtils.Log(MessageBuilder.Trace($"Removed subscription id #{sceneObjectId}"));
            }

            RemoveSubscriberForSubscribers(sceneObjectId);
        }

        public void RemoveSubscriptionForAllSubscribers(int sceneObjectId)
        {
            if (!sceneObjects.ContainsKey(sceneObjectId))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObjectId} does not exists in a region."), LogMessageType.Error);
                return;
            }

            RemoveAllSubscribersForSubscriber(sceneObjectId);

            sceneObjects.Remove(sceneObjectId);

            RemoveSubscriberForAllSubscribers(sceneObjectId);
        }

        public bool HasSubscription(int sceneObjectId)
        {
            return sceneObjects.ContainsKey(sceneObjectId);
        }

        public IEnumerable<ISceneObject> GetAllSubscribers()
        {
            return sceneObjects.Values;
        }

        /// <summary>
        /// Add all subscribers for a new subscriber.
        /// </summary>
        /// <param name="sceneObject">A new subscriber</param>
        private void AddSubscribersForSubscriber(ISceneObject sceneObject)
        {
            var subscribers = GetAllSubscribers().Where(subscriber => subscriber.Id != sceneObject.Id).ToArray();
            if (subscribers.Length <= 0) return;
            {
                var subscriberArea = sceneObject.Container.GetComponent<IInterestArea>();
                subscriberArea?.InvokeSubscribersAdded(subscribers);
            }
        }

        /// <summary>
        /// Remove subscribers for the subscriber that left this region.
        /// </summary>
        /// <param name="sceneObject">A removed subscriber</param>
        private void RemoveSubscribersForSubscriber(ISceneObject sceneObject)
        {
            var subscribersForRemoveList = new List<int>();

            foreach (var subscriber in GetAllSubscribers())
            {
                var subscriberArea = subscriber.Container.GetComponent<IInterestArea>();

                var subscribedPublishers = subscriberArea?.GetSubscribedPublishers().ToArray();
                if (subscribedPublishers == null)
                {
                    continue;
                }

                foreach (var publisher in subscribedPublishers)
                {
                    if (!publisher.HasSubscription(sceneObject.Id))
                    {
                        subscribersForRemoveList.Add(subscriber.Id);
                    }
                }

                foreach (var publisher in subscribedPublishers)
                {
                    if (publisher.HasSubscription(sceneObject.Id))
                    {
                        subscribersForRemoveList.Remove(subscriber.Id);
                    }
                }
            }

            if (subscribersForRemoveList.Count <= 0) return;
            {
                var subscriberArea = sceneObject.Container.GetComponent<IInterestArea>();
                subscriberArea?.InvokeSubscribersRemoved(subscribersForRemoveList.ToArray());
            }
        }

        /// <summary>
        /// Add a new subscriber for all other subscribers.
        /// </summary>
        /// <param name="sceneObject">A new subscriber</param>
        private void AddSubscriberForSubscribers(ISceneObject sceneObject)
        {
            var subscribers = GetAllSubscribers().Where(subscriber => subscriber.Id != sceneObject.Id).ToArray();

            foreach (var subscriber in subscribers)
            {
                var subscriberArea = subscriber.Container.GetComponent<IInterestArea>();
                subscriberArea?.InvokeSubscriberAdded(sceneObject);
            }
        }

        /// <summary>
        /// Remove the subscriber that left from this region for other subscribers.
        /// </summary>
        /// <param name="sceneObjectId">A removed subscriber id</param>
        private void RemoveSubscriberForSubscribers(int sceneObjectId)
        {
            foreach (var subscriber in GetAllSubscribers())
            {
                var subscriberArea = subscriber.Container.GetComponent<IInterestArea>();
                if (subscriberArea == null)
                {
                    continue;
                }

                if (!subscriberArea.GetSubscribedPublishers().Any(publisher => publisher.HasSubscription(sceneObjectId)))
                {
                    subscriberArea.InvokeSubscriberRemoved(sceneObjectId);
                }
            }
        }

        /// <summary>
        /// Remove the subscriber that left from this region for all subscribers.
        /// </summary>
        /// <param name="sceneObjectId">A removed subscriber id</param>
        private void RemoveSubscriberForAllSubscribers(int sceneObjectId)
        {
            foreach (var subscriber in GetAllSubscribers())
            {
                var subscriberArea = subscriber.Container.GetComponent<IInterestArea>();
                subscriberArea?.InvokeSubscriberRemoved(sceneObjectId);
            }
        }

        /// <summary>
        /// Remove subscribers for the subscriber that left from this region.
        /// </summary>
        /// <param name="sceneObjectId">A removed subscriber id</param>
        private void RemoveAllSubscribersForSubscriber(int sceneObjectId)
        {
            var subscribers = sceneObjects.Keys.ToArray();

            if (sceneObjects[sceneObjectId] == null) return;
            {
                var subscriberArea = sceneObjects[sceneObjectId].Container.GetComponent<IInterestArea>();
                subscriberArea?.InvokeSubscribersRemoved(subscribers);
            }
        }
    }
}