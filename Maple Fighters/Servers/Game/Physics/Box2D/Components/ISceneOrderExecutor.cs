using CommonTools.Coroutines;

namespace Physics.Box2D
{
    public interface ISceneOrderExecutor
    {
        ICoroutinesExecutor GetPreUpdateExecutor();
        ICoroutinesExecutor GetUpdateExecutor();
        ICoroutinesExecutor GetPostUpdateExecutor();
    }
}