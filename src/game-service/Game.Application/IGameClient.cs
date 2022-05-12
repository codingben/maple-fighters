using System;
using Game.Application.Components;

namespace Game.Application
{
    public interface IGameClient : IDisposable
    {
        int Id { get; }

        IComponents Components { get; }
    }
}