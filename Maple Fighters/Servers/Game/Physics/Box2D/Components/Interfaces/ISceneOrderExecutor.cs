using CommonTools.Coroutines;

namespace Physics.Box2D.Components.Interfaces
{
    public interface ISceneOrderExecutor
    {
        ICoroutinesExecutor GetPreUpdateExecutor();
        ICoroutinesExecutor GetUpdateExecutor();
        ICoroutinesExecutor GetPostUpdateExecutor();
    }
}