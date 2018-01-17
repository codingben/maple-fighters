using System;

namespace ComponentModel.Common
{
    public class Component : IDisposable
    {
        protected IContainer Entity { get; private set; }

        public void Awake(IContainer entity)
        {
            Entity = entity;

            OnAwake();
        }

        public void Dispose()
        {
            OnDestroy();
        }

        protected virtual void OnAwake()
        {
            // Left blank intentionally
        }

        protected virtual void OnDestroy()
        {
            // Left blank intentionally
        }
    }

    public class Component<TOwner> : IDisposable
        where TOwner : IEntity
    {
        protected TOwner Entity { get; private set; }

        public void Awake(TOwner entity)
        {
            Entity = entity;

            OnAwake();
        }

        public void Dispose()
        {
            OnDestroy();
        }

        protected virtual void OnAwake()
        {
            // Left blank intentionally
        }

        protected virtual void OnDestroy()
        {
            // Left blank intentionally
        }
    }
}