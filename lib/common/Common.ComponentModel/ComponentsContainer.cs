using System;
using System.Collections.Generic;
using System.Linq;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel
{
    /// <summary>
    /// A container which contains the components.
    /// </summary>
    public sealed class ComponentsContainer : IComponents
    {
        private readonly List<IComponent> components;

        public ComponentsContainer()
        {
            components = new List<IComponent>();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponents.Add{T}"/> for more information.
        /// </summary>
        /// <exception cref="ComponentAlreadyExistsException">
        /// A component exists in a collection.
        /// </exception>
        TComponent IComponents.Add<TComponent>(TComponent component)
        {
            var isExists = components.OfType<TComponent>().Any();
            if (isExists)
            {
                throw new ComponentAlreadyExistsException(typeof(TComponent).Name);
            }

            component.Awake(this);
            components.Add(component);

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponents.Remove{T}"/> for more information.
        /// </summary>
        void IComponents.Remove<TComponent>()
        {
            var component = components.OfType<TComponent>().FirstOrDefault();
            if (component == null)
            {
                throw new ComponentNotFoundException(typeof(TComponent).Name);
            }

            component.Dispose();

            var index = components.IndexOf(component);
            if (index != -1)
            {
                components.RemoveAt(index);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponents.Get{T}"/> for more information.
        /// </summary>
        TComponent IComponents.Get<TComponent>()
        {
            Utils.ThrowExceptionIfNotInterface<TComponent>();

            var component = components.OfType<TComponent>().FirstOrDefault();

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IDisposable.Dispose"/> for more information.
        /// </summary>
        public void Dispose()
        {
            foreach (var component in components)
            {
                var disposable = component as IDisposable;
                disposable?.Dispose();
            }

            components.Clear();
        }
    }
}