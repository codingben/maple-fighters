using System;

namespace InterestManagement
{
    public interface IInterestArea<out TSceneObject> : IDisposable
        where TSceneObject : ISceneObject
    {
        /// <summary>
        /// Gets the area of interest of the events.
        /// </summary>
        INearbySceneObjectsEvents<TSceneObject> NearbySceneObjectsEvents { get; }
    }
}