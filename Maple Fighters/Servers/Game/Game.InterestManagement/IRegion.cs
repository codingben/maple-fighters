using System.Collections.Generic;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IRegion
    {
        Rectangle PublisherArea { get; }

        void AddSubscription(InterestArea subscriberArea);
        void RemoveSubscription(int subscriberId);
        void RemoveSubscriptionForOtherOnly(int subscriberId);

        bool HasSubscription(int subscriberId);

        IEnumerable<InterestArea> GetAllSubscribersArea();
    }
}