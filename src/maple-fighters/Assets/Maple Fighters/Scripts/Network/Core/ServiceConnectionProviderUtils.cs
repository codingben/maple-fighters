using UnityEngine;

namespace Scripts.Services
{
    using Utils = UI.Utils;

    public static class ServiceConnectionProviderUtils
    {
        public static void OnOperationFailed()
        {
            Disconnect();

            Utils.ShowExceptionNotice();
        }

        /// <summary>
        /// Disconnecting from all services and removing their services game objects.
        /// </summary>
        public static void Disconnect()
        {
            var monoBehaviours = Object.FindObjectsOfType<MonoBehaviour>();
            foreach (var monoBehaviour in monoBehaviours)
            {
                var serviceConnectionProviderBase = monoBehaviour?.GetComponent<IServiceConnectionProviderBase>();
                serviceConnectionProviderBase?.Dispose();
            }
        }
    }
}