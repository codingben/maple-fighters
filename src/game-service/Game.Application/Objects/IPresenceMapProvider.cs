using System;
using Game.Application.Components;

namespace Game.Application.Objects
{
    public interface IPresenceMapProvider
    {
        event Action<IGameScene> MapChanged;

        void SetMap(IGameScene gameScene);

        IGameScene GetMap();
    }
}