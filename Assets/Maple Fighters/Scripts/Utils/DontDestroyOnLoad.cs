using UnityEngine;

namespace Scripts.Utils
{
    public class DontDestroyOnLoad<T> : MonoBehaviour
        where T : DontDestroyOnLoad<T>
    {
        private static T _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            _instance = this as T;
        }
    }
}