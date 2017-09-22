using UnityEngine;

namespace Scripts.UI
{
    public class MonoSingleton<T> : MonoBehaviour
        where T : MonoSingleton<T>
    {
        public static T Instance
        {
            get
            {
                _instance = _instance ?? FindObjectOfType(typeof(T)) as T;
                return _instance;
            }
        }

        private static T _instance;

        private void OnApplicationQuit()
        {
            _instance = null;
        }
    }
}