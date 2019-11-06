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
        public ServerInfo[] ServerInfos;

        public HostingEnvironment Environment;

        public ServerInfo GetServerInfo(ServerType serverType)
        {
            return ServerInfos.FirstOrDefault((x) => x.ServerType == serverType);
        }

        public bool IsProduction()
        {
            return Environment == HostingEnvironment.Production;
        }
    }
}