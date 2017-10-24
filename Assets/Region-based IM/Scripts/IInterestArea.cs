using System;
using System.Collections.Generic;
using Scripts.Gameplay;

namespace InterestManagement.Scripts
{
    public interface IInterestArea
    {
        event Action<ISceneObject> SubscriberAdded;
        event Action<int> SubscriberRemoved;
        event Action<ISceneObject[]> SubscribersAdded;
        event Action<int[]> SubscribersRemoved;

        void InvokeSubscriberAdded(ISceneObject sceneObject);
        void InvokeSubscriberRemoved(int sceneObjectId);
        void InvokeSubscribersAdded(ISceneObject[] sceneObjects);
        void InvokeSubscribersRemoved(int[] sceneObjectIds);

        IEnumerable<IRegion> GetSubscribedPublishers();
    }
}