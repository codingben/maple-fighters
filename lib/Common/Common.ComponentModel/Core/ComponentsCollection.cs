using System.Collections.Generic;
using System.Linq;

namespace Common.ComponentModel.Core
{
    /// <summary>
    /// A collection of the exposed and unexposed components.
    /// </summary>
    internal sealed class ComponentsCollection
    {
        private readonly List<IComponent> exposedComponents;
        private readonly List<IComponent> unexposedComponents;
        
        public ComponentsCollection()
        {
            exposedComponents = new List<IComponent>();
            unexposedComponents = new List<IComponent>();
        }

        /// <summary>
        /// A pointer to the one of the component collection.
        /// </summary>
        /// <param name="exposedState">Determines the collection.</param>
        /// <returns>An exposed or unexposed components.</returns>
        public List<IComponent> this[ExposedState exposedState] =>
            exposedState == ExposedState.Exposable
                ? exposedComponents
                : unexposedComponents;

        /// <summary>
        /// Checks the existence of the given component.
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <param name="exposedState">Determines the collection.</param>
        /// <returns>If the component exists or not.</returns>
        public bool IsExists<TComponent>(ExposedState exposedState)
            where TComponent : IComponent
        {
            var collection = this[exposedState];
            return collection.OfType<TComponent>().Any();
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