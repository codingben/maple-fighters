using System;
using System.Collections.Generic;

namespace Game.InterestManagement
{
    /// <summary>
    /// Represents the nearby scene objects on the interest area.
    /// </summary>
    public interface INearbySceneObjectsCollection : IDisposable
    {
        /// <summary>
        /// Occurs when a new scene object has been entered to the region.
        /// </summary>
        event Action<ISceneObject> SceneObjectAdded;

        /// <summary>
        /// Occurs when a scene object has been left the region.
        /// </summary>
        event Action<ISceneObject> SceneObjectRemoved;

        /// <summary>
        /// Sends the new visible scene objects when entered the region.
        /// </summary>
        event Action<IEnumerable<ISceneObject>> SceneObjectsAdded;

        /// <summary>
        /// Sends the invisible scene objects when left the region.
        /// </summary>
        event Action<IEnumerable<ISceneObject>> SceneObjectsRemoved;
    }
}