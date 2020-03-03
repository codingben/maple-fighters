using System.Collections.Generic;
using Common.ComponentModel;
using CommonTools.Coroutines;

namespace Common.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class CoroutinesExecutor : ComponentBase, ICoroutinesExecutor
    {
        public int Count => executor.Count;

        private readonly ICoroutinesExecutor executor;

        public CoroutinesExecutor(ICoroutinesExecutor executor)
        {
            this.executor = executor;
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            executor?.Dispose();
        }

        public ICoroutine StartCoroutine(
            IEnumerator<IYieldInstruction> coroutineEnumerator)
        {
            return executor.StartCoroutine(coroutineEnumerator);
        }

        public void StopAllCoroutines()
        {
            executor.StopAllCoroutines();
        }
    }
}