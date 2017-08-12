using System.Collections.Generic;
using CommonTools.Coroutines;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Coroutines
{
    public static class ExtensionMethods
    {
        public static ExternalCoroutinesExecuter ExecuteExternally(this ExternalCoroutinesExecuter executer)
        {
            CoroutinesWrappersUpdater.GetInstance().AddCoroutineExecuter(executer);
            return executer;
        }

        public static ExternalCoroutinesExecuter RemoveFromExternalExecuter(this ExternalCoroutinesExecuter executer)
        {
            CoroutinesWrappersUpdater.GetInstance().RemoveCoroutineExecuter(executer);
            return executer;
        }
    }

    public sealed class CoroutinesWrappersUpdater : MonoBehaviour
    {
        private readonly List<ExternalCoroutinesExecuter> coroutinesWrappers = new List<ExternalCoroutinesExecuter>();

        private static CoroutinesWrappersUpdater _instance;

        public static CoroutinesWrappersUpdater GetInstance()
        {
            _instance = _instance ?? new GameObject(typeof(CoroutinesWrappersUpdater).ToString(), typeof(CoroutinesWrappersUpdater)).GetComponent<CoroutinesWrappersUpdater>();
            return _instance;
        }

        public void AddCoroutineExecuter(ExternalCoroutinesExecuter coroutineWrapper)
        {
            coroutinesWrappers.Add(coroutineWrapper);
        }

        public void RemoveCoroutineExecuter(ExternalCoroutinesExecuter coroutineWrapper)
        {
            coroutinesWrappers.Remove(coroutineWrapper);
        }

        private void Awake()
        {
            _instance = this;

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
            _instance = null;
        }
    }
}