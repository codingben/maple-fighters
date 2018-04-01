using System;
using System.Collections.Generic;

namespace Game.InterestManagement
{
    public interface IInterestArea
    {
        event Action<ISceneObject> SubscriberAdded;
        event Action<int> SubscriberRemoved;
        event Action<ISceneObject[]> SubscribersAdded;
        event Action<int[]> SubscribersRemoved;

        void InvokeSubscriberAdded(ISceneObject sceneObject);
        void InvokeSubscriberRemoved(int sceneObjectId);
        void InvokeSubscribersAdded(IEnumerable<ISceneObject> sceneObjects);
        void InvokeSubscribersRemoved(IEnumerable<int> sceneObjectIds);

        void SetSize();
        void DetectOverlapsWithRegions();

        IEnumerable<IRegion> GetSubscribedPublishers();
    }
}