using System;
using System.Collections.Generic;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a container which contains the components collection.
    /// </summary>
    public interface IComponentsContainer : IDisposable
    {
        /// <summary>
        /// Adds a new component which is exposed and if not exists.
        /// </summary>
        /// <exception cref="ComponentAlreadyExistsException{T}">
        /// A component exists in a collection.
        /// </exception>
        /// <typeparam name="TComponent">The given component type.</typeparam>
        /// <param name="component">The new component.</param>
        void Add<TComponent>(TComponent component)
            where TComponent : class;

        /// <summary>
        /// Adds a new component which is exposed and if not exists.
        /// </summary>
        /// <exception cref="ComponentAlreadyExistsException{T}">
        /// A component exists in a collection.
        /// </exception>
        /// <typeparam name="TComponent">The given component type.</typeparam>
        /// <param name="component">The new component.</param>
        void AddExposedOnly<TComponent>(TComponent component)
            where TComponent : class;

        /// <summary>
        /// Removes the given component if found on a collection.
        /// </summary>
        /// <typeparam name="TComponent">The given component type.</typeparam>
        /// <returns>A found component before it was removed.</returns>
        TComponent Remove<TComponent>()
            where TComponent : class;

        /// <summary>
        /// Searches for a component everywhere in the collection.
        /// </summary>
        /// <typeparam name="TComponent">The given component type.</typeparam>
        /// <returns>A determined component by his lifetime.</returns>
        TComponent Find<TComponent>()
            where TComponent : class;

        /// <summary>
        /// Searches for an exposed component in the collection.
        /// </summary>
        /// <typeparam name="TComponent">The given component type.</typeparam>
        /// <returns>A determined component by his lifetime.</returns>
        TComponent FindExposedOnly<TComponent>()
            where TComponent : class;

        /// <summary>
        /// Provides all the components in the collection.
        /// </summary>
        /// <returns>All the components.</returns>
        IEnumerable<object> GetAll();
    }
}