using System.Collections.Generic;
using CommonTools.Coroutines;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Coroutines
{
    public static class ExtensionMethods
    {
        public static ExternalCoroutinesExecutor ExecuteExternally(this ExternalCoroutinesExecutor executer)
        {
            CoroutinesWrappersUpdater.GetInstance().AddCoroutineExecuter(executer);
            return executer;
        }

        public static ExternalCoroutinesExecutor RemoveFromExternalExecuter(this ExternalCoroutinesExecutor executer)
        {
            CoroutinesWrappersUpdater.GetInstance().RemoveCoroutineExecuter(executer);
            return executer;
        }
    }

    public sealed class CoroutinesWrappersUpdater : MonoBehaviour
    {
        private readonly List<ExternalCoroutinesExecutor> coroutinesWrappers = new List<ExternalCoroutinesExecutor>();

        public static CoroutinesWrappersUpdater GetInstance()
        {
            instance = instance ?? new GameObject(typeof(CoroutinesWrappersUpdater).ToString(), typeof(CoroutinesWrappersUpdater)).GetComponent<CoroutinesWrappersUpdater>();
            return instance;
        }

        private static CoroutinesWrappersUpdater instance;

        public void AddCoroutineExecuter(ExternalCoroutinesExecutor coroutineWrapper)
        {
            coroutinesWrappers.Add(coroutineWrapper);
        }

        public void RemoveCoroutineExecuter(ExternalCoroutinesExecutor coroutineWrapper)
        {
            coroutinesWrappers.Remove(coroutineWrapper);
        }

        private void Awake()
        {
            instance = this;

            TimeProviders.DefaultTimeProvider = new GameTimeProvider();
        }

        private void Update()
        {
            if (coroutinesWrappers.Count != 0)
            {
                coroutinesWrappers.ForEach(x => x.Update());
            }
        }

        private void OnApplicationQuit()
        {
            if (coroutinesWrappers.Count != 0)
            {
                coroutinesWrappers.ForEach(x => x.StopAllCoroutines());
            }

            instance = null;
        }
    }
}