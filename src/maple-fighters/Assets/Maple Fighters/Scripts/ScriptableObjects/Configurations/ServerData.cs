using System;

namespace ScriptableObjects.Configurations
{
    [Serializable]
    public class ServerData
    {
        public string Name;

        public ServerType ServerType;

        public string Url;
    }
}