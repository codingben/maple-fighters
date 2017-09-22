using Scripts.Services;

namespace Scripts.Containers
{
    public interface IServiceContainer
    {
        IGameService GameService { get; }
    }
}