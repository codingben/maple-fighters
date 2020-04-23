using System;

namespace InterestManagement
{
    public interface IInterestArea<out TObject> : IDisposable
        where TObject : ISceneObject
    {
        /// <summary>
        /// Gets the area of interest of the events.
        /// </summary>
        INearbySceneObjectsEvents<TObject> NearbySceneObjectsEvents { get; }
    }
}