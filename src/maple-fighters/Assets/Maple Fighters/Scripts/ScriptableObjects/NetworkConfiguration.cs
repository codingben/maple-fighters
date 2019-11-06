using System.Linq;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "NetworkConfiguration",
        menuName = "Scriptable Objects/NetworkConfiguration",
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