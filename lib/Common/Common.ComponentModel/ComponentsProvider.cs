using System;
using Common.ComponentModel.Core;

namespace Common.ComponentModel
{
    /// <summary>
    /// Represents a components container.
    /// </summary>
    public sealed class ComponentsProvider : IComponents, IExposedComponents
    {
        private readonly IComponentsContainer components;

        public ComponentsProvider()
        {
            components = new ComponentsContainer();
        }

        TComponent IComponents.Add<TComponent>(TComponent component)
        {
            components.Add(component);

            component.Awake(this);

            return component;
        }

        TComponent IExposedComponents.Add<TComponent>(TComponent component)
        {
            components.AddExposedOnly(component);

            component.Awake(this);

            return component;
        }

        void IComponents.Remove<TComponent>()
        {
            var component = components.Remove<TComponent>();
            component.Dispose();
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