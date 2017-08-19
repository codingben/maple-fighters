namespace ServerApplication.Common.ComponentModel
{
    public interface IComponentsContainer
    {
        object AddComponent<T>(T component)
            where T : IComponent;

        void RemoveComponent<T>()
            where T : IComponent;

        object GetComponent<T>()
            where T : IComponent;
    }
}