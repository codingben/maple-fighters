using System.Collections.Generic;
using CommonTools.Coroutines;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;

namespace ServerApplication.Common.Components.Coroutines
{
    public class CoroutinesExecutor : Component<IServerEntity>, ICoroutinesExecutor
    {
        public int Count => executer.Count;

        private readonly ICoroutinesExecutor executer;

        public CoroutinesExecutor(ICoroutinesExecutor executer)
        {
            this.executer = executer;
        }

        public ICoroutine StartCoroutine(IEnumerator<IYieldInstruction> coroutineEnumerator)
        {
            return executer.StartCoroutine(coroutineEnumerator);
        }

        public void StopAllCoroutines()
        {
            executer.StopAllCoroutines();
        }

        public new void Dispose()
        {
            executer?.Dispose();
        }
    }
}