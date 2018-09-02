using System;
using Common.ComponentModel.Core;

namespace Common.ComponentModel
{
    public sealed class ComponentsProvider : IComponentsProvider,
                                             IExposedComponentsProvider
    {
        private readonly IComponentsContainer components;

        public ComponentsProvider()
        {
            components = new ComponentsContainer();
        }

        TComponent IComponentsProvider.Add<TComponent>(TComponent component)
        {
            components.TryAdd(component);

            if (component is IComponent componentBase)
            {
                componentBase.Awake(this);
            }

            return component;
        }

        TComponent IExposedComponentsProvider.Add<TComponent>(TComponent component)
        {
            components.TryAddExposedOnly(component);

            if (component is IComponent componentBase)
            {
                componentBase.Awake(this);
            }

            return component;
        }

        void IComponentsProvider.Remove<TComponent>()
        {
            var component = components.Remove<TComponent>();
            if (component is IComponent componentBase)
            {
                componentBase.Dispose();
            }
        }

        public TComponent Get<TComponent>()
            where TComponent : class
        {
            TComponent component = null;

            if (ComponentUtils.IsInterface<TComponent>())
            {
                component = components.Find<TComponent>();
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