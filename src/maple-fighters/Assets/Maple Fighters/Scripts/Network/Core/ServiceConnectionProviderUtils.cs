using UnityEngine;

namespace Scripts.Services
{
    public static class ServiceConnectionProviderUtils
    {
        public static void OperationFailed()
        {
            foreach (var monoBehaviour in
                Object.FindObjectsOfType<MonoBehaviour>())
            {
                var serviceConnectionProviderBase =
                    monoBehaviour
                        .GetComponent<IServiceConnectionProviderBase>();
                serviceConnectionProviderBase?.Dispose();
            }

            // TODO: Utils.ShowExceptionNotice();
        }
    }
}