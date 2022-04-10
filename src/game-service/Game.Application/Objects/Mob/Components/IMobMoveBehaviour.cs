using Coroutines;

namespace Game.Application.Objects.Components
{
    public interface IMobMoveBehaviour
    {
        void SetCoroutineRunner(ICoroutineRunner coroutineRunner);

        void StartMove();

        void StopMove();
    }
}