using System;

namespace Game.Application.Components
{
    public interface IGameScene : IDisposable
    {
        IComponents Components { get; }
    }
}