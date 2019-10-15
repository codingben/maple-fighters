using CommonTools.Coroutines;
using UnityEngine;

namespace Network.Utils
{
    public class DefaultTimeProviderSetter : MonoBehaviour
    {
        private void Awake()
        {
            TimeProviders.DefaultTimeProvider = new GameTimeProvider();

            Destroy(gameObject);
        }
    }
}