using System.Collections.Generic;

namespace InterestManagement.Scripts
{
    public interface IInterestArea
    {
        IEnumerable<IRegion> GetSubscribedPublishers();
    }
}