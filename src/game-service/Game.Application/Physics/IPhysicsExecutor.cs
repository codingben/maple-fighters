using System;
using Coroutines;

namespace Game.Physics
{
    public interface IPhysicsExecutor : IDisposable
    {
        ICoroutineRunner GetCoroutineRunner();
    }
}