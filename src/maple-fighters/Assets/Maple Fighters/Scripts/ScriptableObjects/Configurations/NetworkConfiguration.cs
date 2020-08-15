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

        public ServerData GetServerData(ServerType serverType)
        {
            return ServerData.FirstOrDefault((x) => x.ServerType == serverType);
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