using CommonTools.Coroutines;
using UnityEngine;

namespace Scripts.Coroutines
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