using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "NetworkConfiguration",
        menuName = "Scriptable Objects/NetworkConfiguration",
        order = 3)]
    public class NetworkConfiguration : ScriptableSingleton<NetworkConfiguration>
    {
        public HostingEnvironment Environment;

        public bool IsProduction()
        {
            return Environment == HostingEnvironment.Production;
        }

        public bool IsDevelopment()
        {
            return Environment == HostingEnvironment.Development;
        }
    }
}