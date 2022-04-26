using Game.Application.Objects;

namespace Game.Application.Components
{
    public interface IRemotePlayerProvider
    {
        bool AddToPresenceScene();

        bool RemoveFromPresenceScene();

        IGameObject Provide();
    }
}