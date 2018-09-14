using Config.Net;
using ServerCommon.Configuration.Definitions;

namespace ServerCommon.Configuration
{
    public static class ServerConfiguration
    {
        public static void Setup()
        {
            SetupPeerConfiguration();
        }

        public static void SetupPeerConfiguration()
        {
            ServerSettings.Peer = new ConfigurationBuilder<IPeer>()
                .UseInMemoryConfig()
                .Build();
        }
    }
}