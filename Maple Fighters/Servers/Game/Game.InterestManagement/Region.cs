using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Region : IRegion
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

            AddSubscribersForSubscriber(sceneObject);
            AddSubscriberForSubscribers(sceneObject);

            LogUtils.Log(MessageBuilder.Trace($"Added subscription id #{sceneObject.Id}"));
        }

        public void RemoveSubscription(int sceneObjectId)
        {
            if (!sceneObjects.ContainsKey(sceneObjectId))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObjectId} does not exists in a region."), LogMessageType.Error);
                return;
            }

            RemoveSubscribersForSubscriber(sceneObjectId);

            sceneObjects.Remove(sceneObjectId);

            RemoveSubscriberForSubscribers(sceneObjectId);

            LogUtils.Log(MessageBuilder.Trace($"Removed subscription id #{sceneObjectId}"));
        }

        public void RemoveSubscriptionForAllSubscribers(int sceneObject)
        {
            if (!sceneObjects.ContainsKey(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObject} does not exists in a region."), LogMessageType.Error);
                return;
            }

            RemoveAllSubscribersForSubscriber(sceneObject);

            sceneObjects.Remove(sceneObject);

            RemoveSubscriberForAllSubscribers(sceneObject);
        }

        public bool HasSubscription(int sceneObject)
        {
            return sceneObjects.ContainsKey(sceneObject);
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
            var subscribers = sceneObjects.Values.Where(subscriber => subscriber.Id != sceneObject.Id).ToArray();
            var subscriberArea = sceneObject.Container.GetComponent<IInterestArea>().AssertNotNull();
            subscriberArea?.InvokeSubscribersAdded(subscribers); 
        }

        /// <summary>
        /// Remove subscribers for the subscriber that left this region.
        /// </summary>
        /// <param name="sceneObjectId">A removed subscriber id</param>
        private void RemoveSubscribersForSubscriber(int sceneObjectId)
        {
            var subscribers = sceneObjects.Values.Where(subscriber => subscriber.Id != sceneObjectId).ToArray();

            var subscribersForRemoveList = new List<int>();

            foreach (var subscriber in subscribers)
            {
                var subscriberArea = subscriber.Container.GetComponent<IInterestArea>().AssertNotNull();
                if (subscriberArea.GetPublishers().Any(publisher => !publisher.HasSubscription(subscriber.Id)))
                {
                    subscribersForRemoveList.Add(subscriber.Id);
                }
            }

            if (subscribersForRemoveList.Count <= 0) return;
            {
                var subscriberArea = sceneObjects[sceneObjectId].Container.GetComponent<IInterestArea>().AssertNotNull();
                subscriberArea.InvokeSubscribersRemoved(subscribersForRemoveList.ToArray());
            }
        }

        /// <summary>
        /// Add a new subscriber for all other subscribers.
        /// </summary>
        /// <param name="sceneObject">A new subscriber</param>
        private void AddSubscriberForSubscribers(ISceneObject sceneObject)
        {
            var subscribers = sceneObjects.Values.Where(subscriber => subscriber.Id != sceneObject.Id).ToArray();

            foreach (var subscriber in subscribers)
            {
                var subscriberArea = subscriber.Container.GetComponent<IInterestArea>().AssertNotNull();
                subscriberArea.InvokeSubscriberAdded(sceneObject);
            }
        }

        /// <summary>
        /// Remove the subscriber that left from this region for other subscribers.
        /// </summary>
        /// <param name="sceneObjectId">A removed subscriber id</param>
        private void RemoveSubscriberForSubscribers(int sceneObjectId)
        {
            foreach (var subscriber in sceneObjects.Values)
            {
                var subscriberArea = subscriber.Container.GetComponent<IInterestArea>().AssertNotNull();
                if (!subscriberArea.GetPublishers().Any(publisher => publisher.HasSubscription(sceneObjectId)))
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
            foreach (var subscriber in sceneObjects.Values)
            {
                var subscriberArea = subscriber.Container.GetComponent<IInterestArea>().AssertNotNull();
                subscriberArea.InvokeSubscriberRemoved(sceneObjectId);
            }
        }

        /// <summary>
        /// Remove subscribers for the subscriber that left from this region.
        /// </summary>
        /// <param name="sceneObjectId">A removed subscriber id</param>
        private void RemoveAllSubscribersForSubscriber(int sceneObjectId)
        {
            var subscribers = sceneObjects.Keys.Where(id => id != sceneObjectId).ToArray();

            if (sceneObjects[sceneObjectId] == null) return;
            {
                var subscriberArea = sceneObjects[sceneObjectId].Container.GetComponent<IInterestArea>().AssertNotNull();
                subscriberArea.InvokeSubscribersRemoved(subscribers);
            }
        }
    }
}