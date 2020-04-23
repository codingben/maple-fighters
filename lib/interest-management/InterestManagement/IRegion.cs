using System;
using System.Collections.Generic;
using Common.MathematicsHelper;

namespace InterestManagement
{
    public interface IRegion<TObject> : IDisposable
        where TObject : ISceneObject
    {
        /// <summary>
        /// Occurs when a new subscriber enters the region.
        /// </summary>
        event Action<TObject> SubscriberAdded;

        /// <summary>
        /// Occurs when a subscriber leaves the region.
        /// </summary>
        event Action<TObject> SubscriberRemoved;

        /// <summary>
        /// Adds a new scene object to the region.
        /// </summary>
        /// <param name="sceneObject">The actual scene object.</param>
        void Subscribe(TObject sceneObject);

        /// <summary>
        /// Removes the scene object from the region.
        /// </summary>
        /// <param name="sceneObject">The scene object.</param>
        void Unsubscribe(TObject sceneObject);

        /// <summary>
        /// Gets all the subscribed scene objects to this region.
        /// </summary>
        /// <returns>All the relevant scene objects.</returns>
        IEnumerable<TObject> GetAllSubscribers();

        /// <summary>
        /// Gets the subscriber count.
        /// </summary>
        /// <returns>The count.</returns>
        int SubscriberCount();

        /// <summary>
        /// Checks if the region overlaps with the other rectangle.
        /// </summary>
        /// <param name="position">The other position.</param>
        /// <param name="size">The other size.</param>
        /// <returns>If overlaps or not.</returns>
        bool IsOverlaps(Vector2 position, Vector2 size);
    }
}