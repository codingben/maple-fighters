using System;
using System.Collections.Generic;
using System.Linq;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel.Core
{
    /// <inheritdoc />
    /// <summary>
    /// A container which contains the components.
    /// </summary>
    public sealed class ComponentsContainer : IComponentsContainer
    {
        private readonly ComponentsCollection components;

        public ComponentsContainer()
        {
            components = new ComponentsCollection();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponentsContainer.Add{T}"/> for more information.
        /// </summary>
        public void Add<TComponent>(TComponent component)
            where TComponent : class
        {
            var exposedState =
                ComponentsContainerUtils.GetExposedState<TComponent>();
            var isExists = components.IsExists<TComponent>(exposedState);
            if (!isExists)
            {
                components[exposedState].Add(component);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponentsContainer.AddExposedOnly{T}"/> for more information.
        /// </summary>
        public void AddExposedOnly<TComponent>(TComponent component)
            where TComponent : class
        {
            var exposedState =
                ComponentsContainerUtils.GetExposedState<TComponent>();
            if (exposedState == ExposedState.Exposable)
            {
                var isExists = components.IsExists<TComponent>(
                    ExposedState.Exposable);
                if (!isExists)
                {
                    components[ExposedState.Exposable].Add(component);
                }
            }
            else
            {
                throw new ComponentNotExposedException<TComponent>();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponentsContainer.Remove{T}"/> for more information.
        /// </summary>
        public TComponent Remove<TComponent>()
            where TComponent : class
        {
            var exposedState =
                ComponentsContainerUtils.GetExposedState<TComponent>();
            var collection = components[exposedState];

            var component = collection.OfType<TComponent>()
                .FirstOrDefault();
            if (component == null)
            {
                throw new ComponentNotFoundException<TComponent>();
            }

            var index = collection.IndexOf(component);
            if (index != -1)
            {
                collection.RemoveAt(index);
            }

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponentsContainer.Find{T}"/> for more information.
        /// </summary>
        public TComponent Find<TComponent>()
            where TComponent : class
        {
            var component = components.GetAllComponents().OfType<TComponent>()
                .FirstOrDefault();
            if (component != null)
            {
                ComponentsContainerUtils.SetComponentByLifetime(ref component);
            }

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponentsContainer.FindExposedOnly{T}"/> for more information.
        /// </summary>
        public TComponent FindExposedOnly<TComponent>()
            where TComponent : class
        {
            var component = components.GetExposedComponents().OfType<TComponent>()
                .FirstOrDefault();
            if (component != null)
            {
                ComponentsContainerUtils.SetComponentByLifetime(ref component);
            }
            else
            {
                throw new ComponentNotFoundException<TComponent>();
            }

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponentsContainer.GetAll"/> for more information.
        /// </summary>
        public IEnumerable<object> GetAll()
        {
            return components.GetAllComponents();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IDisposable.Dispose"/> for more information.
        /// </summary>
        public void Dispose()
        {
            components.Clear();
        }
    }
}