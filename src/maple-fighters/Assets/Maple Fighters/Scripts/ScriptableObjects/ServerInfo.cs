using System;

namespace Scripts.ScriptableObjects
{
    [Serializable]
    public class ServerInfo
    {
        public string Name;

        public ServerType ServerType;

        public string IpAdress;

        public int Port;
    }
}