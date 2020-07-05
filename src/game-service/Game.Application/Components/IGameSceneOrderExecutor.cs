using Coroutines;

namespace Game.Application.Components
{
    public interface IGameSceneOrderExecutor
    {
        CoroutineRunner GetCoroutineRunner();
    }
}