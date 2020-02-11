using System;
using System.Collections.Generic;

namespace Game.InterestManagement
{
    public interface INearbySceneObjectsEvents<out TObject> : IDisposable
        where TObject : ISceneObject
    {
        /// <summary>
        /// Occurs when a new scene object has been entered to the region.
        /// </summary>
        event Action<TObject> SceneObjectAdded;

        /// <summary>
        /// Occurs when a scene object has been left the region.
        /// </summary>
        event Action<TObject> SceneObjectRemoved;

        /// <summary>
        /// Sends the new visible scene objects when entered the region.
        /// </summary>
        event Action<IEnumerable<TObject>> SceneObjectsAdded;

        /// <summary>
        /// Sends the invisible scene objects when left the region.
        /// </summary>
        event Action<IEnumerable<TObject>> SceneObjectsRemoved;
    }
}