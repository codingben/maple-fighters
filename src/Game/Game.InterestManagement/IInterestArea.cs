using System;

namespace Game.InterestManagement
{
    /// <summary>
    /// Supposed to be attached to the scene object to detect nearby regions.
    /// </summary>
    public interface IInterestArea : IDisposable
    {
        /// <summary>
        /// Gets the nearby scene objects collection.
        /// </summary>
        /// <returns>
        /// The <see cref="INearbySubscriberCollection"/>.
        /// </returns>
        INearbySubscriberCollection GetNearbySubscribers();
    }
}