using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using Scripts.Gameplay;
using UnityEngine;

namespace InterestManagement.Scripts
{
    public class Region : MonoBehaviour, IRegion
    {
        public int Id { get; set; }
        public Rectangle PublisherArea { get; set; }

        private readonly HashSet<ISceneObject> sceneObjects = new HashSet<ISceneObject>();

        private void Update()
        {
            name = $"Region Id: {Id} Entities: {sceneObjects.Count}";
        }

        public void AddSubscription(ISceneObject sceneObject)
        {
            if (!sceneObjects.Add(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObject.Id} already exists in a region."), LogMessageType.Warning);
                return;
            }

            AddSubscribersForSubscriber(sceneObject);
            AddSubscriberForSubscribers(sceneObject);

            LogUtils.Log(MessageBuilder.Trace($"Added subscription id #{sceneObject.Id} to Region id #{Id}"));
        }

        public void RemoveSubscription(ISceneObject sceneObject)
        {
            if (!sceneObjects.Remove(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObject.Id} does not exist in a region."), LogMessageType.Warning);
                return;
            }

            RemoveSubscribersForSubscriber(sceneObject);
            RemoveSubscriberForSubscribers(sceneObject);

            LogUtils.Log(MessageBuilder.Trace($"Removed subscription id #{sceneObject.Id} from Region id #{Id}"));
        }

        public void RemoveSubscriptionForAllSubscribers(ISceneObject sceneObject)
        {
            if (!sceneObjects.Remove(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObject.Id} does not exist in a region."), LogMessageType.Warning);
                return;
            }

            RemoveAllSubscribersForSubscriber(sceneObject);
            RemoveSubscriberForAllSubscribers(sceneObject);

            LogUtils.Log(MessageBuilder.Trace($"Removed subscription id #{sceneObject.Id} from Region id #{Id}"));
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
                var subscriberArea = sceneObject.GetGameObject().GetComponent<IInterestAreaEvents>();
                subscriberArea?.AddSubscribers(subscribers);
            }
        }

        /// <summary>
        /// Remove subscribers for the subscriber that left this region.
        /// </summary>
        /// <param name="sceneObject">A removed subscriber</param>
        private void RemoveSubscribersForSubscriber(ISceneObject sceneObject)
        {
            var subscribersToRemoveList = new List<ISceneObject>();

            foreach (var subscriber in GetAllSubscribers())
            {
                var subscriberArea = subscriber.GetGameObject().GetComponent<IInterestArea>();
                var subscribedPublishers = subscriberArea?.GetSubscribedPublishers();
                if (subscribedPublishers == null)
                {
                    continue;
                }

                var hasSubscription = subscribedPublishers.Any(x => x.HasSubscription(sceneObject));
                if (!hasSubscription)
                {
                    subscribersToRemoveList.Add(subscriber);
                }
            }

            if (subscribersToRemoveList.Count <= 0) return;
            {
                var subscriberArea = sceneObject.GetGameObject().GetComponent<IInterestAreaEvents>();
                subscriberArea.RemoveSubscribers(subscribersToRemoveList);
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
                var subscriberArea = subscriber.GetGameObject().GetComponent<IInterestAreaEvents>();
                subscriberArea?.AddSubscriber(sceneObject);
            }
        }

        /// <summary>
        /// Remove the subscriber that left from this region for other subscribers.
        /// </summary>
        private void RemoveSubscriberForSubscribers(ISceneObject sceneObject)
        {
            foreach (var subscriber in sceneObjects)
            {
                var subscriberArea = subscriber.GetGameObject().GetComponent<IInterestArea>();
                var hasSubscription = subscriberArea?.GetSubscribedPublishers().Any(publisher => publisher.HasSubscription(sceneObject));
                if (hasSubscription != null && !hasSubscription.Value)
                {
                    var interestAreaEvents = subscriber.GetGameObject().GetComponent<IInterestAreaEvents>();
                    interestAreaEvents?.RemoveSubscriber(sceneObject);
                }
            }
        }

        /// <summary>
        /// Remove the subscriber that left from this region for all subscribers.
        /// </summary>
        private void RemoveSubscriberForAllSubscribers(ISceneObject sceneObject)
        {
            foreach (var subscriber in GetAllSubscribers())
            {
                var subscriberArea = subscriber.GetGameObject().GetComponent<IInterestAreaEvents>();
                subscriberArea.RemoveSubscriber(sceneObject);
            }
        }

        /// <summary>
        /// Remove subscribers for the subscriber that left from this region.
        /// </summary>
        private void RemoveAllSubscribersForSubscriber(ISceneObject sceneObject)
        {
            var subscriberArea = sceneObject.GetGameObject().GetComponent<IInterestAreaEvents>();
            subscriberArea?.RemoveSubscribers(sceneObjects);
        }

        public bool HasSubscription(ISceneObject sceneObject) => sceneObjects.Contains(sceneObject);

        public ISceneObject[] GetSubscribersExcept(int id) => sceneObjects.Where(subscriber => subscriber.Id != id).ToArray();
        public IEnumerable<ISceneObject> GetAllSubscribers() => sceneObjects;
    }
}