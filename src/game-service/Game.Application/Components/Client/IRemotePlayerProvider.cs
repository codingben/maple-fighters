using Game.Application.Objects;

namespace Game.Application.Components
{
    public interface IRemotePlayerProvider
    {
        IGameObject Provide();
    }
}