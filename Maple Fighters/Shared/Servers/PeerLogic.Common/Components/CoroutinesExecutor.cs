using System.Collections.Generic;
using CommonTools.Coroutines;
using ComponentModel.Common;

namespace PeerLogic.Common.Components
{
    public class CoroutinesExecutor : Component<IPeerEntity>, ICoroutinesExecutor
    {
        public int Count => executer.Count;

        private readonly CommonTools.Coroutines.ICoroutinesExecutor executer;

        public CoroutinesExecutor(CommonTools.Coroutines.ICoroutinesExecutor executer)
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