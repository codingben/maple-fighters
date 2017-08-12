using System.Collections.Generic;
using CommonTools.Coroutines;

namespace Scripts.Coroutines
{
    public sealed class CoroutinesExecuterWrapper : ICoroutinesExecuterWrapper, ICoroutinesExecuter
    {
        private readonly ExternalCoroutinesExecuter coroutinesExecuter;

        public int Count => coroutinesExecuter.Count;

        public CoroutinesExecuterWrapper()
        {
            coroutinesExecuter = new ExternalCoroutinesExecuter();

            CoroutinesWrappersUpdater.GetInstance().AddCoroutineWrapper(this);
        }

        public void UpdateCoroutines()
        {
            coroutinesExecuter?.Update();
        }

        public ICoroutine StartCoroutine(IEnumerator<IYieldInstruction> coroutineEnumerator)
        {
            return coroutinesExecuter.StartCoroutine(coroutineEnumerator);
        }

        public void StopAllCoroutines()
        {
            coroutinesExecuter.StopAllCoroutines();
        }

        public void Dispose()
        {
            coroutinesExecuter.Dispose();

            CoroutinesWrappersUpdater.GetInstance().RemoveCoroutineWrapper(this);
        }
    }
}