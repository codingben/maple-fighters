using System;
using Common.ComponentModel.Core;

namespace Common.ComponentModel.Generic
{
    public sealed class OwnerComponentsProvider<TOwner> : IComponentsProvider,
                                                          IExposedComponentsProvider
        where TOwner : class
    {
        private readonly TOwner owner;
        private readonly IComponentsContainer components;

        public OwnerComponentsProvider(TOwner owner)
        {
            this.owner = owner;
            components = new ComponentsContainer();
        }

        TComponent IComponentsProvider.Add<TComponent>(TComponent component)
        {
            components.TryAdd(component);

            if (component is IComponent<TOwner> componentBase)
            {
                componentBase.Awake(owner, this);
            }

            return component;
        }

        TComponent IExposedComponentsProvider.Add<TComponent>(TComponent component)
        {
            components.TryAddExposedOnly(component);

            if (component is IComponent<TOwner> componentBase)
            {
                componentBase.Awake(owner, this);
            }

            return component;
        }

        void IComponentsProvider.Remove<TComponent>()
        {
            var component = components.Remove<TComponent>();
            if (component is IComponent<TOwner> componentBase)
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