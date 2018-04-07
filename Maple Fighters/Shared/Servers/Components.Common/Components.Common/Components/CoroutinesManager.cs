using System.Collections.Generic;
using CommonTools.Coroutines;
using ComponentModel.Common;
using Components.Common.Interfaces;

namespace Components.Common
{
    public class CoroutinesManager : Component, ICoroutinesManager
    {
        public int Count => executor.Count;

        private readonly ICoroutinesExecutor executor;

        public CoroutinesManager(ICoroutinesExecutor executor)
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