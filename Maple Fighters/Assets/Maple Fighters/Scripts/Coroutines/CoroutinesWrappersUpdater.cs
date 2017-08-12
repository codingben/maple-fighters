using System.Collections.Generic;
using CommonTools.Coroutines;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Coroutines
{
    public sealed class CoroutinesWrappersUpdater : MonoBehaviour
    {
        private readonly List<ICoroutinesExecuterWrapper> coroutinesWrappers = new List<ICoroutinesExecuterWrapper>();

        private static CoroutinesWrappersUpdater _instance;

        public static CoroutinesWrappersUpdater GetInstance()
        {
            Debug.Assert(_instance != null, "CoroutinesWrapperUpdater::Instance -> Could not find an instance of this class -> Creating a new one.");
            _instance = _instance ?? new GameObject(typeof(CoroutinesWrappersUpdater).ToString(), typeof(CoroutinesWrappersUpdater)).GetComponent<CoroutinesWrappersUpdater>();
            return _instance;
        }

        public void AddCoroutineWrapper(CoroutinesExecuterWrapper coroutineWrapper)
        {
            coroutinesWrappers.Add(coroutineWrapper);
        }

        public void RemoveCoroutineWrapper(CoroutinesExecuterWrapper coroutineWrapper)
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
                coroutinesWrappers.ForEach(x => x.UpdateCoroutines());
            }
        }

        private void OnApplicationQuit()
        {
            _instance = null;
        }
    }
}