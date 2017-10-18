using System.Collections.Generic;
using MathematicsHelper;

namespace Game.InterestManagement
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