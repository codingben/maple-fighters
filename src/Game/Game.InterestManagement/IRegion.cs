using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    /// <summary>
    /// Represents the region in the scene with scene objects.
    /// </summary>
    public interface IRegion
    {
        /// <summary>
        /// Gets the geometric shape of the region.
        /// </summary>
        IRectangle Rectangle { get; }

        /// <summary>
        /// Adds a new scene object to the region.
        /// </summary>
        /// <param name="sceneObject">The actual scene object.</param>
        void Subscribe(ISceneObject sceneObject);

        /// <summary>
        /// Removes the scene object from the region.
        /// </summary>
        /// <param name="sceneObject">The scene object.</param>
        void Unsubscribe(ISceneObject sceneObject);

        /// <summary>
        /// Gets all the subscribed scene objects to this region.
        /// </summary>
        /// <returns>All the relevant scene objects.</returns>
        IEnumerable<ISceneObject> GetAllSubscribers();
    }
}