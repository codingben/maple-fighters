namespace ServerApplication.Common.ComponentModel
{
    public interface IComponentsContainer
    {
        T AddComponent<T>(T component)
            where T : IComponent;

        void RemoveComponent<T>()
            where T : IComponent;

        T GetComponent<T>()
            where T : IComponent;

        void RemoveAllComponents();
    }
}