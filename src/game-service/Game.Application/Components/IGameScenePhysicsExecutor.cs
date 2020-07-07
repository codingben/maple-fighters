using System;
using Coroutines;

namespace Game.Application.Components
{
    public interface IGameScenePhysicsExecutor : IDisposable
    {
        CoroutineRunner GetCoroutineRunner();
    }
}