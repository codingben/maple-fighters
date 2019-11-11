using System;
using CommonTools.Coroutines;
using UnityEngine;

namespace Network.Utils
{
    public class DefaultTimeProviderSetter : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                if (TimeProviders.DefaultTimeProvider == null)
                {
                    TimeProviders.DefaultTimeProvider = new GameTimeProvider();
                }
            }
            catch (Exception)
            {
                // Left blank intentionally
            }

            Destroy(gameObject);
        }
    }
}