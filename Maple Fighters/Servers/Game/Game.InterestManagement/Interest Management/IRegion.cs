using System.Collections.Generic;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;

namespace InterestManagement
{
    public interface IRegion
    {
        Rectangle PublisherArea { get; }

        void AddSubscription(ISceneObject sceneObject);
        void RemoveSubscription(int sceneObjectId);
        void RemoveSubscriptionForAllSubscribers(int sceneObjectId);

        bool HasSubscription(int sceneObjectId);

        IEnumerable<ISceneObject> GetAllSubscribers();
    }
}