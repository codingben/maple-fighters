using Config.Net;
using ServerCommon.Configuration.Definitions;

namespace ServerCommon.Configuration
{
    public static class ServerConfiguration
    {
        static ServerConfiguration()
        {
            InitializePeerConfiguration();
        }

        private static void InitializePeerConfiguration()
        {
            ServerSettings.Peer = new ConfigurationBuilder<IPeer>()
                .UseInMemoryConfig()
                .Build();
        }
    }
}