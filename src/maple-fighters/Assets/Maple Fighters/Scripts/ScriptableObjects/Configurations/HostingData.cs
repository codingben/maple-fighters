using System;

namespace ScriptableObjects.Configurations
{
    [Serializable]
    public class HostingData
    {
        public string Name;

        public string Host;

        public HostingEnvironment Environment;
    }
}