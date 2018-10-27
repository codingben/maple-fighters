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
        /// Adds a new scene object to the region.
        /// </summary>
        /// <param name="sceneObject">The actual scene object.</param>
        /// <returns>If succeed or not.</returns>
        bool Subscribe(ISceneObject sceneObject);

        /// <summary>
        /// Removes the scene object from the region.
        /// </summary>
        /// <param name="sceneObject">The scene object.</param>
        /// <returns>If succeed or not.</returns>
        bool Unsubscribe(ISceneObject sceneObject);

        /// <summary>
        /// Provides the information about the position and size of the region.
        /// </summary>
        /// <returns>The rectangle.</returns>
        Rectangle GetRectangle();

        /// <summary>
        /// Gets all the subscribed scene objects to this region.
        /// </summary>
        /// <returns>All the relevant scene objects.</returns>
        IEnumerable<ISceneObject> GetAllSceneObjects();
    }
}