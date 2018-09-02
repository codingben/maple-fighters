using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ComponentModel.Core
{
    public sealed class ComponentsContainer : IComponentsContainer
    {
        private readonly ComponentsCollection<object> components;

        public ComponentsContainer()
        {
            components = new ComponentsCollection<object>();
        }

        public void TryAdd<TComponent>(TComponent component)
            where TComponent : class
        {
            var exposedState = ComponentUtils.GetExposedState<TComponent>();
            var isExists = components.IsExists<TComponent>(exposedState);
            if (!isExists)
            {
                components[exposedState].Add(component);
            }
        }

        public void TryAddExposedOnly<TComponent>(TComponent component)
            where TComponent : class
        {
            if (ComponentUtils.IsExposed<TComponent>())
            {
                var isExists = components.IsExists<TComponent>(
                    ExposedState.Exposable);
                if (!isExists)
                {
                    components[ExposedState.Exposable].Add(component);
                }
            }
        }

        public TComponent Remove<TComponent>()
            where TComponent : class
        {
            var exposedState = ComponentUtils.GetExposedState<TComponent>();
            var collection = components[exposedState];

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

        public TComponent Find<TComponent>()
            where TComponent : class
        {
            var component = components.GetAllComponents().OfType<TComponent>()
                .FirstOrDefault();
            if (component != null)
            {
                component = ProvideComponentByLifeTime(component);
            }

            return component;
        }

        public IEnumerable<object> GetAll()
        {
            return components.GetAllComponents();
        }

        public void Dispose()
        {
            components.Clear();
        }

        private TComponent ProvideComponentByLifeTime<TComponent>(
            TComponent component) where TComponent : class
        {
            var lifeTime = ComponentUtils.GetLifeTime<TComponent>();

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