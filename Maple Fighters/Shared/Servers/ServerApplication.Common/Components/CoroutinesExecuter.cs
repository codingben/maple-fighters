using System.Collections.Generic;
using CommonTools.Coroutines;
using ComponentModel.Common;

namespace ServerApplication.Common.Components.Coroutines
{
    // TODO: Update to Proutines
    public interface ICoroutinesExecuter : ICoroutinesExecutor, IExposableComponent
    {
        // Left blank intentionally
    }

    public class CoroutinesExecutor : Component, ICoroutinesExecuter
    {
        public int Count => executor.Count;

        private readonly ICoroutinesExecutor executor;

        public CoroutinesExecutor(ICoroutinesExecutor executor)
        {
            this.executor = executor;
        }

        public ICoroutine StartCoroutine(IEnumerator<IYieldInstruction> coroutineEnumerator)
        {
            return executor.StartCoroutine(coroutineEnumerator);
        }

        public void StopAllCoroutines()
        {
            executor.StopAllCoroutines();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            executor?.Dispose();
        }
    }
}