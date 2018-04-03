using System;
using System.Collections.Generic;

namespace InterestManagement.Components.Interfaces
{
    public interface IInterestAreaEvents
    {
        event Action<ISceneObject> SubscriberAdded;
        event Action<int> SubscriberRemoved;
        event Action<ISceneObject[]> SubscribersAdded;
        event Action<int[]> SubscribersRemoved;

        void AddSubscriber(ISceneObject sceneObject);
        void RemoveSubscriber(int sceneObjectId);
        void AddSubscribers(IEnumerable<ISceneObject> sceneObjects);
        void RemoveSubscribers(IEnumerable<int> sceneObjectIds);
    }
}