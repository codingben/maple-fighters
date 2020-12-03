using System.Collections.Generic;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;

namespace InterestManagement
{
    public interface IRegion
    {
        Rectangle PublisherArea { get; }

        void AddSubscription(ISceneObject sceneObject);
        void RemoveSubscription(ISceneObject sceneObject);
        void RemoveSubscriptionForAllSubscribers(ISceneObject sceneObject);

        bool HasSubscription(ISceneObject sceneObject);

        IEnumerable<ISceneObject> GetAllSubscribers();
    }
}