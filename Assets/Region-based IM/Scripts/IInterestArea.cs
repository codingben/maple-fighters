using System.Collections.Generic;

namespace InterestManagement.Scripts
{
    public interface IInterestArea
    {
        void SetSize();
        void DetectOverlapsWithRegions();

        IEnumerable<IRegion> GetSubscribedPublishers();
    }
}