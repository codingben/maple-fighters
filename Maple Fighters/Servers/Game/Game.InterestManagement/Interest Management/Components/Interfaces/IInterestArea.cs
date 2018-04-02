using System.Collections.Generic;

namespace InterestManagement.Components.Interfaces
{
    public interface IInterestArea
    {
        void SetSize();
        void DetectOverlapsWithRegions();

        IEnumerable<IRegion> GetSubscribedPublishers();
    }
}