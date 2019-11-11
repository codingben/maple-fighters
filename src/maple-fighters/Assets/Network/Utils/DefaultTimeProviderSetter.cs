using CommonTools.Coroutines;
using UnityEngine;

namespace Network.Utils
{
    public class DefaultTimeProviderSetter : MonoBehaviour
    {
        private void Awake()
        {
            if (TimeProviders.DefaultTimeProvider == null)
            {
                TimeProviders.DefaultTimeProvider = new GameTimeProvider();
            }

            Destroy(gameObject);
        }
    }
}