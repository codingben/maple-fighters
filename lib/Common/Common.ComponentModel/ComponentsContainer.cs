using System;
using System.Collections.Generic;
using System.Linq;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel
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
            var exposedState = Utils.GetExposedState<TComponent>();
            var isExists = components.IsExists<TComponent>(exposedState);
            if (isExists)
            {
                throw new ComponentAlreadyExistsException(typeof(TComponent).Name);
            }

            component.Awake(this);

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
            var exposedState = Utils.GetExposedState<TComponent>();
            if (exposedState == ExposedState.Exposable)
            {
                var isExists =
                    components.IsExists<TComponent>(ExposedState.Exposable);
                if (isExists)
                {
                    throw new ComponentAlreadyExistsException(typeof(TComponent).Name);
                }

                component.Awake(this);

                components[ExposedState.Exposable].Add(component);
            }
            else
            {
                throw new ComponentNotExposedException(typeof(TComponent).Name);
            }

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IComponents.Remove{T}"/> for more information.
        /// </summary>
        void IComponents.Remove<TComponent>()
        {
            var exposedState = Utils.GetExposedState<TComponent>();
            var collection = components[exposedState];
            var component = collection.OfType<TComponent>().FirstOrDefault();
            if (component == null)
            {
                throw new ComponentNotFoundException(typeof(TComponent).Name);
            }

            component.Dispose();

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
            Utils.ThrowExceptionIfNotInterface<TComponent>();

            var component = components.GetAllComponents()
                .OfType<TComponent>()
                .FirstOrDefault();

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IExposedComponents.Get{T}"/> for more information.
        /// </summary>
        TComponent IExposedComponents.Get<TComponent>()
        {
            Utils.ThrowExceptionIfNotInterface<TComponent>();

            var component = components.GetExposedComponents()
                .OfType<TComponent>()
                .FirstOrDefault();

            return component;
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IDisposable.Dispose"/> for more information.
        /// </summary>
        public void Dispose()
        {
            var collection = new List<object>(components.GetAllComponents());
            foreach (var component in collection)
            {
                var disposable = component as IDisposable;
                disposable?.Dispose();
            }

            components.Clear();
        }
    }
}