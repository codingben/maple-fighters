using Coroutines;

namespace Game.Application.Components
{
    public interface IGameSceneOrderExecutor
    {
        CoroutineRunner GetBeforeUpdateRunner();

        CoroutineRunner GetDuringUpdateRunner();

        CoroutineRunner GetAfterUpdatedRunner();
    }
}