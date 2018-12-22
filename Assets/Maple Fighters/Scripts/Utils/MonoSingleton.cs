using UnityEngine;

namespace Scripts.Utils
{
    public class MonoSingleton<TObject> : MonoBehaviour
        where TObject : MonoSingleton<TObject>
    {
        public static TObject GetInstance()
        {
            return instance;
        }

        private static TObject instance;

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                instance = this as TObject;

                DontDestroyOnLoad(gameObject);

                OnAwake();
            }
        }

        private void OnDestroy()
        {
            OnDestroying();
        }

        private void OnApplicationQuit()
        {
            OnApplicationQuitting();
        }

        protected virtual void OnAwake()
        {
            // Left blank intentionally
        }

        protected virtual void OnApplicationQuitting()
        {
            instance = null;
        }

        protected virtual void OnDestroying()
        {
            // Left blank intentionally
        }
    }
}