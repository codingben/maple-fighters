using System.Collections.Generic;
using System.Linq;

namespace Common.ComponentModel.Core
{
    public sealed class ComponentsCollection<TComponent>
        where TComponent : class, new()
    {
        private readonly List<TComponent> exposedComponents;
        private readonly List<TComponent> unexposedComponents;

        public ComponentsCollection()
        {
            exposedComponents = new List<TComponent>();
            unexposedComponents = new List<TComponent>();
        }

        public List<TComponent> this[ExposedState exposedState] =>
            exposedState == ExposedState.Exposable
                ? exposedComponents
                : unexposedComponents;

        public bool IsExists<T>(ExposedState exposedState)
            where T : TComponent
        {
            var collection = this[exposedState];
            if (collection.OfType<T>().Any())
            {
                throw new ComponentModelException(
                    $"A component {typeof(T).Name} already exists!");
            }

            return false;
        }

        public IEnumerable<TComponent> GetAllComponents()
        {
            return exposedComponents.Concat(unexposedComponents);
        }

        public IEnumerable<TComponent> GetExposedComponents()
        {
            return exposedComponents;
        }

        public void Clear()
        {
            exposedComponents.Clear();
            unexposedComponents.Clear();
        }
    }
}