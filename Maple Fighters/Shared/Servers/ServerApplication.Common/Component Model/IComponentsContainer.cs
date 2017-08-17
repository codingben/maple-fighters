namespace ServerApplication.Common.ComponentModel
{
    public interface IComponentsContainer
    {
        void AddComponent<T>(T component)
            where T : class, IComponent;

        void RemoveComponent<T>(T component)
            where T : class, IComponent;

        void GetComponent<T>(out T component)
            where T : class, IComponent;
    }
}