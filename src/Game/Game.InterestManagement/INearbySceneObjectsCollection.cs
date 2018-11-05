using System;

namespace Game.InterestManagement
{
    public interface INearbySceneObjectsCollection : IDisposable
    {
        /// <summary>
        /// The notifier of the new nearby scene objects.
        /// </summary>
        event Action<ISceneObject[]> NearbySceneObjectsAdded;

        /// <summary>
        /// The notifier of the removed nearby scene objects.
        /// </summary>
        event Action<ISceneObject[]> NearbySceneObjectsRemoved;
    }
}