using Coroutines;

namespace Game.Application.Components
{
    public interface IGameScenePhysicsExecutor
    {
        CoroutineRunner GetCoroutineRunner();
    }
}