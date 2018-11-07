using System;
using System.Collections.Generic;

namespace Game.InterestManagement
{
    /// <summary>
    /// Represents the nearby scene objects on the interest area.
    /// </summary>
    public interface INearbySubscriberCollection : IDisposable
    {
        /// <summary>
        /// Occurs when a new scene object has been entered to the region.
        /// </summary>
        event Action<ISceneObject> SubscriberAdded;

        /// <summary>
        /// Occurs when a scene object has been left the region.
        /// </summary>
        event Action<ISceneObject> SubscriberRemoved;

        /// <summary>
        /// Sends the new visible scene objects when entered the region.
        /// </summary>
        event Action<IEnumerable<ISceneObject>> SubscribersAdded;

        /// <summary>
        /// Sends the invisible scene objects when left the region.
        /// </summary>
        event Action<IEnumerable<ISceneObject>> SubscribersRemoved;
    }
}