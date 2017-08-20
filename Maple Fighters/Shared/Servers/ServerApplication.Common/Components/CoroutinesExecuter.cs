using System.Collections.Generic;
using CommonTools.Coroutines;
using ServerApplication.Common.ComponentModel;

namespace ServerApplication.Common.Components.Coroutines
{
    public class CoroutinesExecuter : IComponent, ICoroutinesExecuter
    {
        public int Count => executer.Count;

        private readonly ICoroutinesExecuter executer;

        public CoroutinesExecuter(ICoroutinesExecuter executer)
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

        public void Dispose()
        {
            executer?.Dispose();
        }
    }
}