using System;
using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IRegion<TObject> : IDisposable
        where TObject : ISceneObject
    {
        /// <summary>
        /// Gets the geometric shape of the region.
        /// </summary>
        IRectangle Rectangle { get; }

        /// <summary>
        /// The notifier of the new subscriber to the region.
        /// </summary>
        event Action<TObject> SubscriberAdded;

        /// <summary>
        /// The notifier of the removed subscriber from the region.
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
        /// Checks if the region has subscribers.
        /// </summary>
        /// <returns>It has subscribers or not.</returns>
        bool HasSubscribers();
    }
}