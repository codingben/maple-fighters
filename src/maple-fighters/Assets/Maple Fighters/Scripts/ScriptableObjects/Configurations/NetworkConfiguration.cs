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
        public ServerData[] DevServerData;

        public ServerData[] ProdServerData;

        public HostingEnvironment HostingEnvironment;

        public string GetServerUrl(ServerType serverType)
        {
            var url = string.Empty;

            if (IsProduction())
            {
                var serverData =
                    ProdServerData.FirstOrDefault((x) => x.ServerType == serverType);
                if (serverData != null)
                {
                    url = serverData.Url;
                }
            }
            else
            {
                var serverData =
                    DevServerData.FirstOrDefault((x) => x.ServerType == serverType);
                if (serverData != null)
                {
                    url = serverData.Url;
                }
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