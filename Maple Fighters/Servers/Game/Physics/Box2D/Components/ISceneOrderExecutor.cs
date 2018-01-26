using CommonTools.Coroutines;
using ComponentModel.Common;

namespace Physics.Box2D
{
    public interface ISceneOrderExecutor : IExposableComponent
    {
        ICoroutinesExecutor GetPreUpdateExecutor();
        ICoroutinesExecutor GetUpdateExecutor();
        ICoroutinesExecutor GetPostUpdateExecutor();
    }
}