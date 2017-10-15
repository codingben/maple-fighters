namespace Scripts.Containers
{
    public interface IContainers
    {
        IGameObjectsContainer GameObjectsContainer { get; }
        IServiceContainer ServiceContainer { get; }
    }
}