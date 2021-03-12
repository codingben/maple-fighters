using System;
using System.Collections.Generic;

namespace InterestManagement
{
    public interface IInterestArea<TSceneObject> : IDisposable
        where TSceneObject : ISceneObject
    {
        /// <summary>
        /// Sets the source of regions.
        /// </summary>
        /// <param name="matrixRegion">The matrix region.</param>
        void SetMatrixRegion(IMatrixRegion<TSceneObject> matrixRegion);

        /// <summary>
        /// Gets all nearby regions.
        /// </summary>
        /// <returns>The regions.</returns>
        IEnumerable<IRegion<TSceneObject>> GetRegions();

        /// <summary>
        /// Gets all nearby objects in the regions.
        /// </summary>
        /// <returns>The scene objects.</returns>
        IEnumerable<TSceneObject> GetNearbySceneObjects();

        /// <summary>
        /// Gets the notifier of events for nearby scene objects.
        /// </summary>
        /// <returns>The event notifier.</returns>
        INearbySceneObjectsEvents<TSceneObject> GetNearbySceneObjectsEvents();
    }
}