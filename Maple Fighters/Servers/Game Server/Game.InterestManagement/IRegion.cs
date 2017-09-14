using System.Collections.Generic;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IRegion
    {
        Rectangle Area { get; }

        void AddSubscription(IGameObject gameObject);
        void RemoveSubscription(int gameObjectId);
        void RemoveSubscriptionForOtherOnly(int gameObjectId);

        bool HasSubscription(int gameObjectId);

        IEnumerable<IGameObject> GetAllSubscribers();
    }
}