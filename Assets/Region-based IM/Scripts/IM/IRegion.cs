using System.Collections.Generic;
using Scripts.Gameplay;

namespace InterestManagement.Scripts
{
    public interface IRegion
    {
        int Id { get; }

        Rectangle PublisherArea { get; }

        void AddSubscription(ISceneObject sceneObject);
        void RemoveSubscription(ISceneObject sceneObject);
        void RemoveSubscriptionForAllSubscribers(ISceneObject sceneObject);

        bool HasSubscription(ISceneObject sceneObject);

        IEnumerable<ISceneObject> GetAllSubscribers();
    }
}