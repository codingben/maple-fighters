using UnityEngine;

namespace Scripts.UI.Core
{
    public class MonoSingleton<T> : MonoBehaviour
        where T : MonoSingleton<T>
    {
        public static T Instance
        {
            get
            {
                if (isDestroying)
                {
                    return null;
                }

                _instance = _instance ?? FindObjectOfType(typeof(T)) as T;
                return _instance;
            }
        }

        private static T _instance;
        private static bool isDestroying;

        private void OnDestroy()
        {
            OnDestroyed();
        }

        protected virtual void OnDestroyed()
        {
            isDestroying = true;
            _instance = null;
        }

        private void OnApplicationQuit()
        {
            isDestroying = true;
            _instance = null;
        }
    }
}