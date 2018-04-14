using System;
using System.Collections.Generic;
using Scripts.Gameplay;

namespace InterestManagement.Scripts
{
    public interface IInterestAreaEvents
    {
        event Action<ISceneObject> SubscriberAdded;
        event Action<ISceneObject> SubscriberRemoved;
        event Action<ISceneObject[]> SubscribersAdded;
        event Action<ISceneObject[]> SubscribersRemoved;

        void AddSubscriber(ISceneObject sceneObject);
        void RemoveSubscriber(ISceneObject sceneObject);
        void AddSubscribers(IEnumerable<ISceneObject> sceneObjects);
        void RemoveSubscribers(IEnumerable<ISceneObject> sceneObjects);
    }
}