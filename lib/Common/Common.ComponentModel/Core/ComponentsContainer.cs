using System;
using System.Linq;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel.Core
{
    /// <summary>
    /// A container which contains the components.
    /// </summary>
    public sealed class ComponentsContainer : IComponents, IExposedComponents
    {
        private readonly ComponentsCollection components;

        public ComponentsContainer()
        {
            components = new ComponentsCollection();
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
            var exposedState =
                ComponentSettingsUtils.GetExposedState<TComponent>();
            var isExists = components.IsExists<TComponent>(exposedState);
            if (isExists)
            {
                throw new ComponentAlreadyExistsException(nameof(TComponent));
            }

            components[exposedState].Add(component);

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IExposedComponents.Add{T}"/> for more information.
        /// </summary>
        /// <exception cref="ComponentAlreadyExistsException">
        /// A component exists in a collection.
        /// </exception>
        TComponent IExposedComponents.Add<TComponent>(TComponent component)
        {
            var exposedState =
                ComponentSettingsUtils.GetExposedState<TComponent>();
            if (exposedState == ExposedState.Exposable)
            {
                var isExists =
                    components.IsExists<TComponent>(ExposedState.Exposable);
                if (isExists)
                {
                    throw new ComponentAlreadyExistsException(nameof(TComponent));
                }

                components[ExposedState.Exposable].Add(component);
            }
            else
            {
                throw new ComponentNotExposedException(nameof(TComponent));
            }

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponents.Remove{T}"/> for more information.
        /// </summary>
        void IComponents.Remove<TComponent>()
        {
            var exposedState =
                ComponentSettingsUtils.GetExposedState<TComponent>();
            var collection = components[exposedState];

            var component = collection.OfType<TComponent>().FirstOrDefault();
            if (component == null)
            {
                throw new ComponentNotFoundException(nameof(TComponent));
            }

            var index = collection.IndexOf(component);
            if (index != -1)
            {
                collection.RemoveAt(index);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponents.Get{T}"/> for more information.
        /// </summary>
        TComponent IComponents.Get<TComponent>()
        {
            var component = components.GetAllComponents()
                .OfType<TComponent>()
                .FirstOrDefault();
            if (component == null)
            {
                throw new ComponentNotFoundException(nameof(TComponent));
            }

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IExposedComponents.Get{T}"/> for more information.
        /// </summary>
        TComponent IExposedComponents.Get<TComponent>()
        {
            var component = components.GetExposedComponents()
                .OfType<TComponent>()
                .FirstOrDefault();
            if (component == null)
            {
                throw new ComponentNotFoundException(nameof(TComponent));
            }

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IDisposable.Dispose"/> for more information.
        /// </summary>
        public void Dispose()
        {
            foreach (var component in components.GetAllComponents())
            {
                var disposable = component as IDisposable;
                disposable?.Dispose();
            }

            components.Clear();
        }
    }
}