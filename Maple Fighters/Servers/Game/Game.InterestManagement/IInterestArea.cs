using System;
using System.Collections.Generic;
using ComponentModel.Common;

namespace Game.InterestManagement
{
    public interface IInterestArea : IExposableComponent
    {
        event Action<ISceneObject> SubscriberAdded;
        event Action<int> SubscriberRemoved;
        event Action<ISceneObject[]> SubscribersAdded;
        event Action<int[]> SubscribersRemoved;

        void InvokeSubscriberAdded(ISceneObject sceneObject);
        void InvokeSubscriberRemoved(int sceneObjectId);
        void InvokeSubscribersAdded(ISceneObject[] sceneObjects);
        void InvokeSubscribersRemoved(int[] sceneObjectIds);

        void SetSize();
        void DetectOverlapsWithRegions();

        IEnumerable<IRegion> GetSubscribedPublishers();
    }
}