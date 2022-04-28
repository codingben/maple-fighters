using System;
using Game.Application.Components;

namespace Game.Application.Objects
{
    public interface IPresenceSceneProvider
    {
        event Action<IGameScene> SceneChanged;

        void SetScene(IGameScene gameScene);

        IGameScene GetScene();
    }
}