using System.Collections.Generic;
using CommonTools.Coroutines;
using Scripts.Gameplay;
using UnityEngine;

namespace Scripts.Coroutines
{
    public sealed class CoroutinesWrappersUpdater : MonoBehaviour
    {
        public static CoroutinesWrappersUpdater GetOrCreateInstance()
        {
            if (isDestroying)
            {
                return null;
            }

            if (instance == null)
            {
                var name = typeof(CoroutinesWrappersUpdater).Name
                    .MakeSpaceBetweenWords();
                var type = typeof(CoroutinesWrappersUpdater);

                instance = new GameObject(name, type)
                    .GetComponent<CoroutinesWrappersUpdater>();
            }

            return instance;
        }

        private static CoroutinesWrappersUpdater instance;
        private static bool isDestroying;

        private List<ExternalCoroutinesExecutor> coroutinesWrappers;

        private void Awake()
        {
            coroutinesWrappers = new List<ExternalCoroutinesExecutor>();
        }

        private void Update()
        {
            var coroutines = coroutinesWrappers.ToArray();
            if (coroutines.Length != 0)
            {
                foreach (var coroutine in coroutines)
                {
                    coroutine?.Update();
                }
            }
        }

        private void OnDestroy()
        {
            Dispose();

            isDestroying = true;
        }

        private void OnApplicationQuit()
        {
            Dispose();

            isDestroying = true;
        }

        private void Dispose()
        {
            if (coroutinesWrappers.Count != 0)
            {
                coroutinesWrappers.ForEach(x => x.Dispose());
            }

            instance = null;
        }

        public void AddCoroutineExecutor(
            ExternalCoroutinesExecutor coroutineWrapper)
        {
            coroutinesWrappers.Add(coroutineWrapper);
        }

        public void RemoveCoroutineExecutor(
            ExternalCoroutinesExecutor coroutineWrapper)
        {
            coroutinesWrappers.Remove(coroutineWrapper);
        }
    }
}