using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Application.Components
{
    /// <summary>
    /// Collection of components.
    /// </summary>
    public sealed class ComponentCollection : IComponents
    {
        private readonly List<IComponent> components = new();

        public ComponentCollection(IEnumerable<IComponent> collection = null)
        {
            if (collection == null) return;

            components.AddRange(collection);

            foreach (var component in components)
            {
                component.Awake(this);
            }
        }

        /// <summary>
        /// See <see cref="IComponents.Add"/> for more information.
        /// </summary>
        public void Add(IComponent component)
        {
            if (component == null) return;

            components.Add(component);

            component.Awake(this);
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
                    $"Component {name} should be accessible via interface.";

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