using System;

namespace Scripts.Services.GameProviderApi
{
    [Serializable]
    public struct GameData
    {
        public string name;

        public string protocol;

        public string url;
    }
}