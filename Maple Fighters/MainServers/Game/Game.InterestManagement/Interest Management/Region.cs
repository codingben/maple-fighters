using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;
using Config = JsonConfig.Config;

namespace InterestManagement
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

            var debug = (bool)Config.Global.Log.InterestManagement;
            if (debug)
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

            var sceneObject = sceneObjects[sceneObjectId];
            sceneObjects.Remove(sceneObjectId);

            var debug = (bool)Config.Global.Log.InterestManagement;
            if (debug)
            {
                LogUtils.Log(MessageBuilder.Trace($"Removed subscription id #{sceneObjectId}"));
            }

            RemoveSubscribersForSubscriber(sceneObject);
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
        /// <param name="subscriber">A new subscriber</param>
        private void AddSubscribersForSubscriber(ISceneObject subscriber)
        {
            var subscribers = GetAllSubscribers().Where(sceneObject => sceneObject.Id != subscriber.Id).ToArray();
            if (subscribers.Length <= 0) return;
            {
                var interestAreaEvents = subscriber.Components.GetComponent<IInterestAreaEvents>();
                interestAreaEvents?.AddSubscribers(subscribers);
            }
        }

        /// <summary>
        /// Remove subscribers for the subscriber that left this region.
        /// </summary>
        /// <param name="subscriber">A removed subscriber</param>
        private void RemoveSubscribersForSubscriber(ISceneObject subscriber)
        {
            var subscribersForRemoveList = new List<int>();

            foreach (var sceneObject in GetAllSubscribers())
            {
                var hasSubscription = GetSubscribedPublishers(sceneObject)?.Any(x => x.HasSubscription(sceneObject.Id));
                if (hasSubscription != null && !hasSubscription.Value)
                {
                    subscribersForRemoveList.Add(sceneObject.Id);
                }
            }

            if (subscribersForRemoveList.Count <= 0) return;
            {
                var interestAreaEvents = subscriber.Components.GetComponent<IInterestAreaEvents>();
                interestAreaEvents?.RemoveSubscribers(subscribersForRemoveList.ToArray());
            }

            IEnumerable<IRegion> GetSubscribedPublishers(ISceneObject sceneObject)
            {
                var subscriberArea = sceneObject.Components.GetComponent<IInterestArea>();
                var subscribedPublishers = subscriberArea?.GetSubscribedPublishers().ToArray();
                return subscribedPublishers;
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
                var interestAreaEvents = subscriber.Components.GetComponent<IInterestAreaEvents>();
                interestAreaEvents?.AddSubscriber(sceneObject);
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
                var subscriberArea = subscriber.Components.GetComponent<IInterestArea>();
                var hasSubscription = subscriberArea?.GetSubscribedPublishers().Any(publisher => publisher.HasSubscription(sceneObjectId));
                if (hasSubscription != null && !hasSubscription.Value)
                {
                    var interestAreaEvents = subscriber.Components.GetComponent<IInterestAreaEvents>();
                    interestAreaEvents?.RemoveSubscriber(sceneObjectId);
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
                var interestAreaEvents = subscriber.Components.GetComponent<IInterestAreaEvents>();
                interestAreaEvents?.RemoveSubscriber(sceneObjectId);
            }
        }

        /// <summary>
        /// Remove subscribers for the subscriber that left from this region.
        /// </summary>
        /// <param name="sceneObjectId">A removed subscriber id</param>
        private void RemoveAllSubscribersForSubscriber(int sceneObjectId)
        {
            var subscriber = sceneObjects[sceneObjectId];
            if (subscriber == null) return;
            {
                var subscribers = sceneObjects.Keys.ToArray();
                var interestAreaEvents = subscriber.Components.GetComponent<IInterestAreaEvents>();
                interestAreaEvents?.RemoveSubscribers(subscribers);
            }
        }
    }
}