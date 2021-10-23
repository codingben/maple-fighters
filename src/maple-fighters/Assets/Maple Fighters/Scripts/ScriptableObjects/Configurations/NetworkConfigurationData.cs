using System;

namespace ScriptableObjects.Configurations
{
    [Serializable]
    public class NetworkConfigurationData
    {
        public string Environment;
        public string AuthUrl;
        public string GameProviderUrl;
        public string CharacterUrl;
        public string ChatUrl;
    }
}