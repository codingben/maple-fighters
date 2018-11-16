using System;

namespace Game.InterestManagement
{
    public interface IInterestArea : IDisposable
    {
        /// <summary>
        /// Gets the notifier of the interest area events.
        /// </summary>
        INearbySceneObjectsEvents NearbySceneObjectsEvents { get; }
    }
}