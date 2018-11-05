using System;

namespace Game.InterestManagement
{
    /// <summary>
    /// Supposed to be attached to the scene object to detect nearby regions.
    /// </summary>
    public interface IInterestArea : IDisposable
    {
        /// <summary>
        /// The notifier of the new nearby regions.
        /// </summary>
        event Action<IRegion[]> NearbyRegionsAdded;

        /// <summary>
        /// The notifier of the removed nearby regions.
        /// </summary>
        event Action<IRegion[]> NearbyRegionsRemoved;
    }
}