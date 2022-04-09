using System;

namespace Game.Application.Components
{
    public interface IComponents : IDisposable
    {
        /// <summary>
        /// Adds a new component to the collection.
        /// </summary>
        /// <typeparam name="TComponent">The component.</typeparam>
        void Add(IComponent component);

        /// <summary>
        /// Get a component from the collection only through the interface.
        /// </summary>
        /// <typeparam name="TComponent">The component represented by the interface.</typeparam>
        /// <returns>The component.</returns>
        TComponent Get<TComponent>()
            where TComponent : class;
    }
}