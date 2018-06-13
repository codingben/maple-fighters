using System;
using System.Collections.Generic;

namespace InterestManagement.Components.Interfaces
{
    public interface IInterestAreaEvents
    {
        event Action<ISceneObject> SubscriberAdded;
        event Action<ISceneObject> SubscriberRemoved;
        event Action<ISceneObject[]> SubscribersAdded;
        event Action<ISceneObject[]> SubscribersRemoved;

        bool AddSubscriber(ISceneObject sceneObject);
        bool RemoveSubscriber(ISceneObject sceneObject);
        bool AddSubscribers(IEnumerable<ISceneObject> sceneObjects);
        bool RemoveSubscribers(IEnumerable<ISceneObject> sceneObjects);
    }
}