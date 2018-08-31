using System;

namespace Common.ComponentModel
{
    public sealed class ComponentsContainer : IComponentsContainer,
                                              IExposableComponentsContainer
    {
        private readonly IComponentCollections components;

        public ComponentsContainer()
        {
            components = new ComponentCollections();
        }

        TComponent IComponentsContainer.Add<TComponent>(TComponent component)
        {
            components.TryAdd(component);

            if (component is IComponent componentBase)
            {
                componentBase.Awake(this);
            }

            return component;
        }

        TComponent IExposableComponentsContainer.Add<TComponent>(
            TComponent component)
        {
            components.TryAddExposedOnly(component);

            if (component is IComponent componentBase)
            {
                componentBase.Awake(this);
            }

            return component;
        }

        void IComponentsContainer.Remove<TComponent>()
        {
            var component = components.Remove<TComponent>();
            if (component is IComponent componentBase)
            {
                componentBase.Dispose();
            }
        }

        TComponent IComponentsContainer.Get<TComponent>()
        {
            TComponent component = null;

            if (Utils.IsInterface<TComponent>())
            {
                component = components.Find<TComponent>(ExposedState.Unexposable) 
                    ?? components.Find<TComponent>(ExposedState.Exposable);
            }

            return component;
        }

        TComponent IExposableComponentsContainer.Get<TComponent>()
        {
            TComponent component = null;

            if (Utils.IsInterface<TComponent>())
            {
                component = components.Find<TComponent>(ExposedState.Exposable);
            }

            return component;
        }

        void IDisposable.Dispose()
        {
            foreach (var component in components.GetAll())
            {
                (component as IDisposable)?.Dispose();
            }

            components.Dispose();
        }
    }
}