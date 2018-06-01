namespace ComponentModel.Common
{
    public sealed class Container : IContainer
    {
        private readonly IContainer owner;
        private readonly IComponents components = new Components();

        public Container()
        {
            owner = this;
        }

        public T AddComponent<T>(T component)
            where T : IComponent
        {
            if (components.Add(component) != null)
            {
                component.Awake(owner);
                return component;
            }
            return default(T);
        }

        public void RemoveComponent<T>()
            where T : IComponent
        {
            components.Remove<T>();
        }

        public T GetComponent<T>()
            where T : class
        {
            return components.Get<T>();
        }

        public void Dispose()
        {
            components.Dispose();
        }

        public int Count()
        {
            return components.Count();
        }
    }

    public sealed class Container<TOwner> : IContainer<TOwner>
        where TOwner : IEntity<TOwner>
    {
        private readonly TOwner owner;
        private readonly IComponents components = new Components();

        public Container(TOwner owner)
        {
            this.owner = owner;
        }

        public T AddComponent<T>(T component)
            where T : IComponent<TOwner>
        {
            if (components.Add(component) != null)
            {
                component.Awake(owner);
                return component;
            }
            return default(T);
        }

        public void RemoveComponent<T>()
            where T : IComponent<TOwner>
        {
            components.Remove<T>();
        }

        public T GetComponent<T>()
            where T : class
        {
            return components.Get<T>();
        }

        public void Dispose()
        {
            components.Dispose();
        }

        public int Count()
        {
            return components.Count();
        }
    }
}