using System;

namespace Common.ComponentModel
{
    public class ComponentBase : IComponent
    {
        protected IComponentsContainer Components { get; private set; }

        void IComponent.Awake(IComponentsContainer components)
        {
            Components = components;

            OnAwake();
        }

        void IDisposable.Dispose()
        {
            OnRemoved();
        }

        protected virtual void OnAwake()
        {
            // Left blank intentionally
        }

        protected virtual void OnRemoved()
        {
            // Left blank intentionally
        }
    }
}