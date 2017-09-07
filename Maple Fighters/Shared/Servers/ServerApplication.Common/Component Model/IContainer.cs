namespace ServerApplication.Common.ComponentModel
{
    public interface IContainer
    {
        T AddComponent<T>(T component)
            where T : class, IComponent;

        void RemoveComponent<T>()
            where T : IComponent;

        T GetComponent<T>()
            where T : IComponent;

        void RemoveAllComponents();
    }

    public interface IContainer<TOwner>
    {
        T AddComponent<T>(T component)
            where T : Component<TOwner>, IComponent;

        void RemoveComponent<T>()
            where T : IComponent;

        T GetComponent<T>()
            where T : IComponent;

        void RemoveAllComponents();
    }
}