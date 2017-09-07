using System.Collections.Generic;
using CommonTools.Coroutines;
using ServerApplication.Common.ComponentModel;
using Shared.ServerApplication.Common.PeerLogic;

namespace ServerApplication.Common.Components.Coroutines
{
    public class CoroutinesExecutorEntity : Component<IPeerEntity>, ICoroutinesExecutor
    {
        public int Count => executer.Count;

        private readonly ICoroutinesExecutor executer;

        public CoroutinesExecutorEntity(ICoroutinesExecutor executer)
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