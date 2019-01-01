using CommonTools.Coroutines;
using UnityEngine;

namespace Scripts.Services
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