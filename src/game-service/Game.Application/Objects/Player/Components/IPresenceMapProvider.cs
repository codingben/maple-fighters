using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public interface IPresenceMapProvider
    {
        void SetMap(IGameScene gameScene);

        IGameScene GetMap();
    }
}