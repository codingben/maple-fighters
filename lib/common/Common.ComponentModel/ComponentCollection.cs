using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ComponentModel
{
    /// <summary>
    /// Collection of components.
    /// </summary>
    public sealed class ComponentCollection : IComponents
    {
        private readonly IReadOnlyList<IComponent> components;

        public ComponentCollection(IEnumerable<IComponent> collection)
        {
            components = new List<IComponent>(collection);

            foreach (var component in components)
            {
                component.Awake(this);
            }
        }

        /// <summary>
        /// See <see cref="IComponents.Get{T}"/> for more information.
        /// </summary>
        public TComponent Get<TComponent>()
            where TComponent : class
        {
            if (typeof(TComponent).IsInterface == false)
            {
                var name = typeof(TComponent).Name;
                var message =
                    $"The called component {name} is not through an interface.";

                throw new Exception(message);
            }

            return components.OfType<TComponent>().FirstOrDefault();
        }

        /// <summary>
        /// See <see cref="IDisposable.Dispose"/> for more information.
        /// </summary>
        public void Dispose()
        {
            foreach (var component in components)
            {
                component?.Dispose();
            }
        }
    }
}