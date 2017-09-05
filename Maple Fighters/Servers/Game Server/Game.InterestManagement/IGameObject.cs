using System;
using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    public interface IGameObject : IDisposable
    {
        int Id { get; }

        IScene Scene { get; set; }

        IContainer<GameObjectComponent> Entity { get; }
    }
}