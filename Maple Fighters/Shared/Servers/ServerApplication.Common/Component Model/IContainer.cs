namespace ServerApplication.Common.ComponentModel
{
    public interface IContainer<TOwner>
        where TOwner : IEntity
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