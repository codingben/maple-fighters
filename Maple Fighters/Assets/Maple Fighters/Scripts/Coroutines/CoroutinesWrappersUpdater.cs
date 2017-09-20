using System.Collections.Generic;
using CommonTools.Coroutines;
using Scripts.Services;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Coroutines
{
    public static class ExtensionMethods
    {
        public static ExternalCoroutinesExecutor ExecuteExternally(this ExternalCoroutinesExecutor executer)
        {
            CoroutinesWrappersUpdater.GetInstance().AddCoroutineExecutor(executer);
            return executer;
        }

        public static ExternalCoroutinesExecutor RemoveFromExternalExecutor(this ExternalCoroutinesExecutor executer)
        {
            CoroutinesWrappersUpdater.GetInstance().RemoveCoroutineExecutor(executer);
            return executer;
        }
    }

    public sealed class CoroutinesWrappersUpdater : DontDestroyOnLoad<CoroutinesWrappersUpdater>
    {
        private readonly List<ExternalCoroutinesExecutor> coroutinesWrappers = new List<ExternalCoroutinesExecutor>();

        public static CoroutinesWrappersUpdater GetInstance()
        {
            _instance = _instance ?? new GameObject(typeof(CoroutinesWrappersUpdater).ToString(), typeof(CoroutinesWrappersUpdater))
                .GetComponent<CoroutinesWrappersUpdater>();
            return _instance;
        }

        private static CoroutinesWrappersUpdater _instance;

        public void AddCoroutineExecutor(ExternalCoroutinesExecutor coroutineWrapper)
        {
            coroutinesWrappers.Add(coroutineWrapper);
        }

        public void RemoveCoroutineExecutor(ExternalCoroutinesExecutor coroutineWrapper)
        {
            coroutinesWrappers.Remove(coroutineWrapper);
        }

        private void Start()
        {
            _instance = this;

            TimeProviders.DefaultTimeProvider = new GameTimeProvider();
        }

        private void Update()
        {
            var temp = coroutinesWrappers.ToArray();
            if (temp.Length == 0)
            {
                return;
            }

            foreach (var externalCoroutinesExecutor in temp)
            {
                externalCoroutinesExecutor.Update();
            }
        }

        private void OnApplicationQuit()
        {
            if (coroutinesWrappers.Count != 0)
            {
                coroutinesWrappers.ForEach(x => x.Dispose());
            }

            _instance = null;
        }
    }
}