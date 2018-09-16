using System.Collections.Generic;
using System.Linq;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel.Core
{
    /// <summary>
    /// A collection of the exposed and unexposed components.
    /// </summary>
    public sealed class ComponentsCollection
    {
        private readonly List<object> exposedComponents;
        private readonly List<object> unexposedComponents;
        
        public ComponentsCollection()
        {
            exposedComponents = new List<object>();
            unexposedComponents = new List<object>();
        }

        /// <summary>
        /// A pointer to the one of the component collection.
        /// </summary>
        /// <param name="exposedState">Determines the collection.</param>
        /// <returns>An exposed or unexposed components.</returns>
        public List<object> this[ExposedState exposedState] =>
            exposedState == ExposedState.Exposable
                ? exposedComponents
                : unexposedComponents;

        /// <summary>
        /// Checks the existence of the given component.
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <param name="exposedState">Determines the collection.</param>
        /// <exception cref="ComponentAlreadyExistsException{T}">
        /// A component exists in a collection.
        /// </exception>
        /// <returns>If the component exists or not.</returns>
        public bool IsExists<TComponent>(ExposedState exposedState)
            where TComponent : class
        {
            var collection = this[exposedState];
            if (collection.OfType<TComponent>().Any())
            {
                throw new ComponentAlreadyExistsException<TComponent>();
            }

            return false;
        }

        /// <summary>
        /// Unite exposed and unexposed components together.
        /// </summary>
        /// <returns>All the components.</returns>
        public IEnumerable<object> GetAllComponents()
        {
            return exposedComponents.Concat(unexposedComponents);
        }

        /// <summary>
        /// Provides only exposed components from a collection.
        /// </summary>
        /// <returns>All the exposed components.</returns>
        public IEnumerable<object> GetExposedComponents()
        {
            return exposedComponents;
        }

        /// <summary>
        /// Removes all components from this collection.
        /// </summary>
        public void Clear()
        {
            exposedComponents.Clear();
            unexposedComponents.Clear();
        }
    }
}