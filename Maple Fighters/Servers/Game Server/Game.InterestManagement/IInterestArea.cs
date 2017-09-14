using System.Collections.Generic;

namespace Game.InterestManagement
{
    public interface IInterestArea
    {
        IEnumerable<IRegion> GetPublishers();
    }
}