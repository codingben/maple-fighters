using System;
using ExitGames.Client.Photon;

namespace Scripts.ScriptableObjects
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