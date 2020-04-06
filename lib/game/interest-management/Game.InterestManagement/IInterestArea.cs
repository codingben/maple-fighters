using System;

namespace Game.InterestManagement
{
    public interface IInterestArea<out TObject> : IDisposable
        where TObject : ISceneObject
    {
        /// <summary>
        /// Gets the notifier of the interest area events.
        /// </summary>
        INearbySceneObjectsEvents<TObject> NearbySceneObjectsEvents { get; }
    }
}