using System;
using Game.Application.Objects;

namespace Game.Application.Components
{
    public interface IGamePlayer : IDisposable
    {
        void Create();

        IGameObject GetPlayer();
    }
}