using Game.Application.Components;

namespace Game.Application.Objects
{
    public interface IPresenceMapProvider
    {
        void SetMap(IGameScene gameScene);

        IGameScene GetMap();
    }
}