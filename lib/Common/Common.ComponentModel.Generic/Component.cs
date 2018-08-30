namespace Common.ComponentModel.Generic
{
    public class Component<TOwner> : IComponent<TOwner>
        where TOwner : IEntity<TOwner>
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