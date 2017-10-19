using UnityEngine;

namespace Scripts.Utils
{
    public class DontDestroyOnLoad<T> : MonoBehaviour
        where T : DontDestroyOnLoad<T>
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            gameObject.DontDestroyOnLoad();

            Instance = this as T;
        }
    }
}