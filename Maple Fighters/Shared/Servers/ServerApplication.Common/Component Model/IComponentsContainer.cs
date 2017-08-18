namespace ServerApplication.Common.ComponentModel
{
    public interface IComponentsContainer
    {
        object AddComponent<T>(T component)
            where T : class, IComponent;

        void RemoveComponent<T>()
            where T : class, IComponent;

        object GetComponent<T>()
            where T : class, IComponent;
    }
}