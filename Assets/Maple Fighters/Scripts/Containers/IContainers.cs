using Scripts.Containers.Entity;

namespace Scripts.Containers
{
    public interface IContainers
    {
        IEntityContainer EntityContainer { get; }
        IServiceContainer ServiceContainer { get; }
    }
}