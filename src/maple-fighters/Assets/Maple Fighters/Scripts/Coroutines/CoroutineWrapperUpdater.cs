using System.Collections.Generic;
using CommonTools.Coroutines;
using Scripts.Gameplay.Map;
using UnityEngine;

namespace Scripts.Coroutines
{
    public sealed class CoroutineWrapperUpdater : MonoBehaviour
    {
        public static CoroutinesWrappersUpdater GetOrCreateInstance()
        {
            if (isDestroying)
            {
                return null;
            }

            if (instance == null)
            {
                var type = typeof(CoroutinesWrappersUpdater);
                var name = type.Name.MakeSpaceBetweenWords();

                instance = new GameObject(name, type)
                    .GetComponent<CoroutinesWrappersUpdater>();
            }

            return instance;
        }

        private static CoroutinesWrappersUpdater instance;
        private static bool isDestroying;

        private List<ExternalCoroutinesExecutor> coroutines;

        private void Awake()
        {
            coroutines = new List<ExternalCoroutinesExecutor>();
        }

        private void Update()
        {
            foreach (var coroutine in coroutines)
            {
                coroutine?.Update();
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
            instance = null;

            coroutines.ForEach(x => x.Dispose());
        }

        public void AddCoroutineExecutor(ExternalCoroutinesExecutor coroutine)
        {
            coroutines.Add(coroutine);
        }

        public void RemoveCoroutineExecutor(ExternalCoroutinesExecutor coroutine)
        {
            coroutines.Remove(coroutine);
        }
    }
}