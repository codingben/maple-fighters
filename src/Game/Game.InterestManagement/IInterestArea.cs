using System;

namespace Game.InterestManagement
{
    /// <summary>
    /// Supposed to be attached to the scene object to detect nearby regions.
    /// </summary>
    public interface IInterestArea : IDisposable
    {
        /// <summary>
        /// The notifier of the new nearby scene object.
        /// </summary>
        event Action<ISceneObject> NearbySceneObjectAdded;

        /// <summary>
        /// The notifier of the removed nearby scene object.
        /// </summary>
        event Action<ISceneObject> NearbySceneObjectRemoved;
    }
}