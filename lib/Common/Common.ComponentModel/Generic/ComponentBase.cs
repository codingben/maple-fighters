using System;

namespace Common.ComponentModel.Generic
{
    /// <inheritdoc />
    /// <summary>
    /// A component base which every component must inherit from it.
    /// </summary>
    /// <typeparam name="TOwner">An entity which owns this component.</typeparam>
    public class ComponentBase<TOwner> : IComponent<TOwner>
        where TOwner : class
    {
        protected TOwner Owner { get; private set; }

        protected IComponentsProvider Components { get; private set; }
        
        void IComponent<TOwner>.Awake(
            TOwner owner,
            IComponentsProvider components)
        {
            Owner = owner;
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