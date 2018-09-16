using System;

namespace Common.ComponentModel
{
    /// <inheritdoc />
    /// <summary>
    /// Represents an access to the component's container.
    /// </summary>
    public interface IComponentsProvider : IDisposable
    {
        /// <summary>
        /// Adds and awakes a new component to the component's container.
        /// </summary>
        /// <typeparam name="TComponent">The component itself.</typeparam>
        /// <param name="component">The new component.</param>
        /// <returns>The new component after it was added to the container.</returns>
        TComponent Add<TComponent>(TComponent component)
            where TComponent : class;

        /// <summary>
        /// Removes and disposes a component from the component's container.
        /// </summary>
        /// <typeparam name="TComponent">The desired component.</typeparam>
        void Remove<TComponent>()
            where TComponent : class;

        /// <summary>
        /// Searches for the desired component on the component's container.
        /// </summary>
        /// <typeparam name="TComponent">A component represented by an interface.</typeparam>
        /// <returns>The component which will be found.</returns>
        TComponent Get<TComponent>()
            where TComponent : class;
    }
}