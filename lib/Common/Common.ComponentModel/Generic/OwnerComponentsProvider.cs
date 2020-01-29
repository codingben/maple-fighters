using System;
using Common.ComponentModel.Core;

namespace Common.ComponentModel.Generic
{
    /// <summary>
    /// Represents a components container and allows to set an owner.
    /// </summary>
    /// <typeparam name="TOwner">An entity which owns this component.</typeparam>
    public sealed class OwnerComponentsProvider<TOwner> : IComponents, IExposedComponents
        where TOwner : class
    {
        private readonly TOwner owner;
        private readonly IComponentsContainer components;

        public OwnerComponentsProvider(TOwner owner)
        {
            this.owner = owner;
            components = new ComponentsContainer();
        }

        TComponent IComponents.Add<TComponent>(TComponent component)
        {
            components.Add(component);

            if (component is IComponent<TOwner> componentBase)
            {
                componentBase.Awake(owner, this);
            }

            return component;
        }

        TComponent IExposedComponents.Add<TComponent>(TComponent component)
        {
            components.AddExposedOnly(component);

            if (component is IComponent<TOwner> componentBase)
            {
                componentBase.Awake(owner, this);
            }

            return component;
        }

        void IComponents.Remove<TComponent>()
        {
            var component = components.Remove<TComponent>();
            if (component is IComponent<TOwner> componentBase)
            {
                componentBase.Dispose();
            }
        }

        TComponent IComponents.Get<TComponent>()
        {
            Utils.AssertNotInterface<TComponent>();

            var component = components.Find<TComponent>();
            return component;
        }

        TComponent IExposedComponents.Get<TComponent>()
        {
            Utils.AssertNotInterface<TComponent>();

            var component = components.FindExposedOnly<TComponent>();
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