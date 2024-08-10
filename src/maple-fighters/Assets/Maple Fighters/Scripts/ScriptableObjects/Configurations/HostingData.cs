using System;

namespace ScriptableObjects.Configurations
{
    [Serializable]
    public class HostingData
    {
        public string Name;

        public string Protocol;

        public string Host;

        public HostingEnvironment Environment;
    }
}