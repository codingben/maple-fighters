using System.Collections.Generic;
using CommonTools.Coroutines;
using ComponentModel.Common;

namespace PeerLogic.Common.Components
{
    public class CoroutinesExecutor : Component, ICoroutinesExecutor
    {
        public int Count => executor.Count;

        private readonly CommonTools.Coroutines.ICoroutinesExecutor executor;

        public CoroutinesExecutor(CommonTools.Coroutines.ICoroutinesExecutor executor)
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