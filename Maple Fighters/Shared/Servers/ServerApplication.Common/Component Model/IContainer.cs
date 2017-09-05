namespace ServerApplication.Common.ComponentModel
{
    public interface IContainer<in TComoponent>
        where TComoponent : class, IComponent
    {
        void AddComponent(TComoponent component);

        void RemoveComponent<T>()
            where T : IComponent;

        T GetComponent<T>()
            where T : IComponent;

        void RemoveAllComponents();
    }
}