using System;

namespace ScriptableObjects.Configurations
{
    [Serializable]
    public class ServerInfo
    {
        public string Name;

        public ServerType ServerType;

        public string IpAddress;
    }
}