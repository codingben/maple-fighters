using UnityEngine;

namespace Scripts.Services
{
    public static class ServiceConnectionProviderUtils
    {
        public static void OperationFailed()
        {
            Dispose();

            // TODO: Utils.ShowExceptionNotice();
        }
        
        public static void Dispose()
        {
            foreach (var monoBehaviour in 
                Object.FindObjectsOfType<MonoBehaviour>())
            {
                var serviceConnectionProviderBase = 
                    monoBehaviour
                        .GetComponent<IServiceConnectionProviderBase>();
                serviceConnectionProviderBase?.Dispose();
            }
        }
    }
}