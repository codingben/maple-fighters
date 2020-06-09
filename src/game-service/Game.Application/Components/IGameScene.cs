using System;
using Game.Application.Objects;
using InterestManagement;

namespace Game.Application.Components
{
    public interface IGameScene : IDisposable
    {
        PlayerSpawnData PlayerSpawnData { get; set; }

        IScene<IGameObject> GetScene();
    }
}