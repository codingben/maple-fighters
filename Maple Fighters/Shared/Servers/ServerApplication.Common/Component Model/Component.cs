namespace ServerApplication.Common.ComponentModel
{
    public class Component<TOwner> : CommonComponent
    {
        protected TOwner Entity { get; private set; }

        public void Awake(TOwner entity)
        {
            Entity = entity;

            OnAwake();
        }

        protected virtual void OnAwake()
        {
            // Left blank intentionally
        }
    }
}