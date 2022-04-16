using System;

namespace Game.Application.Components
{
    public interface IGameSceneCollection : IDisposable
    {
        bool Add(string name, IGameScene gameScene);

        void Remove(string name);

        bool TryGet(string name, out IGameScene gameScene);
    }
}