using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ComponentModel
{
    public sealed class ComponentCollections : IComponentCollections
    {
        private readonly IReadOnlyDictionary<ExposedState, List<object>> collections;

        public ComponentCollections()
        {
            // TODO: Find a better way to store the collections
            collections = new Dictionary<ExposedState, List<object>>
            {
                {
                    ExposedState.Unexposable, new List<object>()
                },
                {
                    ExposedState.Exposable, new List<object>()
                }
            };
        }

        public void TryAdd<TComponent>(TComponent component)
            where TComponent : class
        {
            var exposedState = Utils.GetComponentExposedState<TComponent>();
            collections[exposedState].AddIfNotExists(component);
        }

        public void TryAddExposedOnly<TComponent>(TComponent component)
            where TComponent : class
        {
            if (Utils.IsComponentExposed<TComponent>())
            {
                collections[ExposedState.Exposable].AddIfNotExists(component);
            }
        }

        public TComponent Remove<TComponent>()
            where TComponent : class
        {
            var exposedState = Utils.GetComponentExposedState<TComponent>();
            var collection = collections[exposedState];

            var component = collection.OfType<TComponent>()
                .FirstOrDefault();
            if (component == null)
            {
                throw new ComponentModelException(
                    $"Could not remove component {typeof(TComponent).Name} because it was not found.");
            }

            var index = collection.IndexOf(component);
            if (index != -1)
            {
                collection.RemoveAt(index);
            }

            return component;
        }

        public TComponent Find<TComponent>(ExposedState exposedState)
            where TComponent : class
        {
            var component = collections[exposedState].OfType<TComponent>()
                .FirstOrDefault();

            return ProvideComponentByLifeTime(component);
        }

        public IEnumerable<object> GetAll()
        {
            return collections[ExposedState.Unexposable]
                .Concat(collections[ExposedState.Exposable]);
        }

        public void Dispose()
        {
            collections[ExposedState.Unexposable].Clear();
            collections[ExposedState.Exposable].Clear();
        }

        private TComponent ProvideComponentByLifeTime<TComponent>(
            TComponent component) where TComponent : class
        {
            var lifeTime = Utils.GetComponentLifeTime<TComponent>();

            switch (lifeTime)
            {
                case LifeTime.Singleton:
                {
                    break;
                }

                case LifeTime.PerThread:
                {
                    throw new NotImplementedException();
                }

                case LifeTime.PerCall:
                {
                    return (TComponent)Activator.CreateInstance(
                        typeof(TComponent));
                }
            }

            return component;
        }
    }
}