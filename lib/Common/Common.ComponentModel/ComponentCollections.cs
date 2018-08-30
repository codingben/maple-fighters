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

        public TComponent Insert<TComponent>(TComponent component)
            where TComponent : class
        {
            var exposedState = Utils.GetComponentExposedState<TComponent>();

            if (collections[exposedState].Contains(component))
            {
                throw new ComponentModelException(
                    $"Component {typeof(TComponent).Name} already exists!");
            }

            collections[exposedState].Add(component);
            return component;
        }

        public TComponent InsertAndExpose<TComponent>(TComponent component)
            where TComponent : class
        {
            var exposedState = Utils.GetComponentExposedState<TComponent>();

            if (Utils.IsComponent<TComponent>() &&
                exposedState != ExposedState.Exposable)
            {
                throw new ComponentModelException(
                    $"Component {typeof(TComponent).Name} should be exposable.");
            }

            if (collections[ExposedState.Exposable].Contains(component))
            {
                throw new ComponentModelException(
                    $"Component {typeof(TComponent).Name} already exists!");
            }

            collections[ExposedState.Exposable].Add(component);
            return component;
        }

        public TComponent Remove<TComponent>()
            where TComponent : class
        {
            var exposedState = Utils.GetComponentExposedState<TComponent>();

            var component = collections[exposedState].OfType<TComponent>()
                .FirstOrDefault();
            if (component == null)
            {
                throw new ComponentModelException(
                    $"Could not remove component {typeof(TComponent).Name} because it does not exist.");
            }

            var index = collections[exposedState]
                .IndexOf(component);
            if (index != -1)
            {
                collections[exposedState].RemoveAt(index);
            }

            return component;
        }

        public TComponent Find<TComponent>(ExposedState exposedState)
            where TComponent : class
        {
            var component = collections[exposedState].OfType<TComponent>()
                .FirstOrDefault();

            var lifeTime = Utils.GetComponentLifeTime<TComponent>();

            switch (lifeTime)
            {
                case LifeTime.Singleton:
                {
                    return component;
                }

                case LifeTime.PerThread:
                {
                    // TODO: Implement
                    break;
                }

                case LifeTime.PerCall:
                {
                    return (TComponent)Activator.CreateInstance(typeof(TComponent));
                }
            }

            return default(TComponent);
        }

        public IEnumerable<object> GetAll()
        {
            var objects = new List<object>();
            objects.AddRange(collections[ExposedState.Unexposable]);
            objects.AddRange(collections[ExposedState.Exposable]);
            return objects.AsReadOnly();
        }

        public void Dispose()
        {
            var collecion = (Dictionary<ExposedState, List<object>>)collections;
            collecion.Clear();
        }
    }
}