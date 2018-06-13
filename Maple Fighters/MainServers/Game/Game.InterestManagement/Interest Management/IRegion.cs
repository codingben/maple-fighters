using System.Collections.Generic;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;

namespace InterestManagement
{
    public interface IRegion
    {
        Rectangle PublisherArea { get; }

        bool AddSubscription(ISceneObject sceneObject);
        bool RemoveSubscription(ISceneObject sceneObject);
        bool RemoveSubscriptionForAllSubscribers(ISceneObject sceneObject);

        bool HasSubscription(ISceneObject sceneObject);

        IEnumerable<ISceneObject> GetAllSubscribers();
    }
}