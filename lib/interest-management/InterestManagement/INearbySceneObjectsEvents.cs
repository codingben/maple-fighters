using System;
using System.Collections.Generic;

namespace InterestManagement
{
    public interface INearbySceneObjectsEvents<out TSceneObject> : IDisposable
        where TSceneObject : ISceneObject
    {
        /// <summary>
        /// Occurs when a new scene object has been entered to the region.
        /// </summary>
        event Action<TSceneObject> SceneObjectAdded;

        /// <summary>
        /// Occurs when a scene object has been left the region.
        /// </summary>
        event Action<TSceneObject> SceneObjectRemoved;

        /// <summary>
        /// Sends the new visible scene objects when entered the region.
        /// </summary>
        event Action<IEnumerable<TSceneObject>> SceneObjectsAdded;

        /// <summary>
        /// Sends the invisible scene objects when left the region.
        /// </summary>
        event Action<IEnumerable<TSceneObject>> SceneObjectsRemoved;
    }
}