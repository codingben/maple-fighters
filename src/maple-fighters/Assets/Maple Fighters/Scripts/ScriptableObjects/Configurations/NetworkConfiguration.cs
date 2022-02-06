using System.Linq;
using ScriptableObjects.Utils;
using UnityEngine;

namespace ScriptableObjects.Configurations
{
    [CreateAssetMenu(
        fileName = "NetworkConfiguration",
        menuName = "Configurations/NetworkConfiguration",
        order = 0)]
    public class NetworkConfiguration : ScriptableSingleton<NetworkConfiguration>
    {
        public HostingData[] HostingData;

        public HostingEnvironment Environment;

        public string GetHost()
        {
            var hostingData =
                HostingData.FirstOrDefault((x) => x.Environment == Environment);
            if (hostingData != null)
            {
                return hostingData.Host;
            }

            return string.Empty;
        }

        public bool IsEditor()
        {
            return Environment == HostingEnvironment.Editor;
        }

        public bool IsDevelopment()
        {
            return Environment == HostingEnvironment.Development;
        }

        public bool IsProduction()
        {
            return Environment == HostingEnvironment.Production;
        }
    }
}