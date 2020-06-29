using Coroutines;

namespace Game.Application.Components
{
    public interface ISceneOrderExecutor
    {
        CoroutineRunner GetBeforeUpdateRunner();

        CoroutineRunner GetDuringUpdateRunner();

        CoroutineRunner GetAfterUpdatedRunner();
    }
}