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
        public ServerData[] ServerData;

        public HostingEnvironment HostingEnvironment;

        public string GetServerUrl(ServerType serverType)
        {
            var url = string.Empty;

            var serverData =
                ServerData.FirstOrDefault((x) => x.ServerType == serverType);
            if (serverData != null)
            {
                url = serverData.Url;
            }

            return url;
        }

        public bool IsProduction()
        {
            return HostingEnvironment == HostingEnvironment.Production;
        }

        public bool IsDevelopment()
        {
            return HostingEnvironment == HostingEnvironment.Development;
        }
    }
}