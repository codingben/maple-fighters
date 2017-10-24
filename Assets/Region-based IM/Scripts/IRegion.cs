using System.Collections.Generic;
using Scripts.Gameplay;

namespace InterestManagement.Scripts
{
    public interface IRegion
    {
        int Id { get; set; }

        Rectangle PublisherArea { get; }

        void AddSubscription(ISceneObject sceneObject);
        void RemoveSubscription(int sceneObjectId);
        void RemoveSubscriptionForAllSubscribers(int sceneObjectId);

        bool HasSubscription(int sceneObjectId);

        IEnumerable<ISceneObject> GetAllSubscribers();
    }
}