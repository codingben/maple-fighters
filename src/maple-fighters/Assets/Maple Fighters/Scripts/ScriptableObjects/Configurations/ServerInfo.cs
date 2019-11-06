using System;
using ExitGames.Client.Photon;

namespace ScriptableObjects.Configurations
{
    [Serializable]
    public class ServerInfo
    {
        public string Name;

        public ServerType ServerType;

        public string IpAddress;

        public int Port;

        public ConnectionProtocol Protocol;
    }
}