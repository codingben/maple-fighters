using CommonTools.Coroutines;
using UnityEngine;

namespace Scripts.Network
{
    public class GameTimeProviderCreator : MonoBehaviour
    {
        private void Awake()
        {
            TimeProviders.DefaultTimeProvider = new GameTimeProvider();

            Destroy(gameObject);
        }
    }
}