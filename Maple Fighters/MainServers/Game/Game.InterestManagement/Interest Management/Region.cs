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

        private readonly HashSet<ISceneObject> sceneObjects = new HashSet<ISceneObject>();

        public Region(Rectangle rectangle) => PublisherArea = rectangle;

        public void AddSubscription(ISceneObject sceneObject)
        {
            if (!sceneObjects.Add(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObject.Id} already exists in a region."), LogMessageType.Warning);
                return;
            }

            Log();

            AddSubscribersForSubscriber();
            AddSubscriberForSubscribers();

            void Log()
            {
                var debug = (bool)Config.Global.Log.InterestManagement;
                if (debug)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Added subscription id #{sceneObject.Id}"));
                }
            }

            // Add all subscribers for a new subscriber.
            void AddSubscribersForSubscriber()
            {
                var subscribers = GetSubscribersExcept(sceneObject.Id);
                if (subscribers.Length <= 0) return;
                {
                    var interestAreaEvents = sceneObject.Components.GetComponent<IInterestAreaEvents>();
                    interestAreaEvents?.AddSubscribers(subscribers);
                }
            }

            // Add a new subscriber for all other subscribers.
            void AddSubscriberForSubscribers()
            {
                var subscribers = GetSubscribersExcept(sceneObject.Id);
                foreach (var subscriber in subscribers)
                {
                    var interestAreaEvents = subscriber.Components.GetComponent<IInterestAreaEvents>();
                    interestAreaEvents?.AddSubscriber(sceneObject);
                }
            }
        }

        public void RemoveSubscription(ISceneObject sceneObject)
        {
            if (!sceneObjects.Remove(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObject.Id} does not exist in a region."), LogMessageType.Warning);
                return;
            }

            Log();

            RemoveSubscribersForSubscriber();
            RemoveSubscriberForSubscribers();

            void Log()
            {
                var debug = (bool)Config.Global.Log.InterestManagement;
                if (debug)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Removed subscription id #{sceneObject.Id}"));
                }
            }

            // Remove subscribers for the subscriber that left this region.
            void RemoveSubscribersForSubscriber()
            {
                var subscribersToRemoveList = new List<ISceneObject>();

                foreach (var subscriber in sceneObjects)
                {
                    var hasSubscription = GetSubscribedPublishers(subscriber)?.Any(x => x.HasSubscription(subscriber));
                    if (hasSubscription != null && !hasSubscription.Value)
                    {
                        subscribersToRemoveList.Add(subscriber);
                    }
                }

                if (subscribersToRemoveList.Count <= 0) return;
                {
                    var interestAreaEvents = sceneObject.Components.GetComponent<IInterestAreaEvents>();
                    interestAreaEvents?.RemoveSubscribers(subscribersToRemoveList);
                }

                IEnumerable<IRegion> GetSubscribedPublishers(ISceneObject subscriber)
                {
                    var subscriberArea = subscriber.Components.GetComponent<IInterestArea>();
                    var subscribedPublishers = subscriberArea?.GetSubscribedPublishers();
                    return subscribedPublishers;
                }
            }

            // Remove the subscriber that left from this region for other subscribers.
            void RemoveSubscriberForSubscribers()
            {
                foreach (var subscriber in sceneObjects)
                {
                    var subscriberArea = subscriber.Components.GetComponent<IInterestArea>();
                    var hasSubscription = subscriberArea?.GetSubscribedPublishers().Any(publisher => publisher.HasSubscription(sceneObject));
                    if (hasSubscription != null && !hasSubscription.Value)
                    {
                        var interestAreaEvents = subscriber.Components.GetComponent<IInterestAreaEvents>();
                        interestAreaEvents?.RemoveSubscriber(sceneObject);
                    }
                }
            }
        }

        public void RemoveSubscriptionForAllSubscribers(ISceneObject sceneObject)
        {
            if (!sceneObjects.Remove(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObject.Id} does not exist in a region."), LogMessageType.Warning);
                return;
            }

            Log();

            RemoveAllSubscribersForSubscriber();
            RemoveSubscriberForAllSubscribers();

            void Log()
            {
                var debug = (bool)Config.Global.Log.InterestManagement;
                if (debug)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Removed subscription id #{sceneObject.Id}"));
                }
            }

            // Remove subscribers for the subscriber that left from this region.
            void RemoveAllSubscribersForSubscriber()
            {
                var interestAreaEvents = sceneObject?.Components.GetComponent<IInterestAreaEvents>();
                interestAreaEvents?.RemoveSubscribers(sceneObjects);
            }

            // Remove the subscriber that left from this region for all subscribers.
            void RemoveSubscriberForAllSubscribers()
            {
                foreach (var subscriber in sceneObjects)
                {
                    var interestAreaEvents = subscriber.Components.GetComponent<IInterestAreaEvents>();
                    interestAreaEvents?.RemoveSubscriber(sceneObject);
                }
            }
        }

        public bool HasSubscription(ISceneObject sceneObject) => sceneObjects.Contains(sceneObject);

        public ISceneObject[] GetSubscribersExcept(int id) => sceneObjects.Where(subscriber => subscriber.Id != id).ToArray();
        public IEnumerable<ISceneObject> GetAllSubscribers() => sceneObjects;
    }
}
 