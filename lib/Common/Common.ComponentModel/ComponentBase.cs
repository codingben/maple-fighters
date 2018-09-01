using System;

namespace Common.ComponentModel
{
    public class ComponentBase : IComponent
    {
        protected IComponentsProvider Components { get; private set; }

        void IComponent.Awake(IComponentsProvider components)
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