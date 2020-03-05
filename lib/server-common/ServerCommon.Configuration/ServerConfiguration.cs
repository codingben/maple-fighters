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
            ServerSettings.InboundPeer =
                new ConfigurationBuilder<IInboundPeer>()
                    .UseInMemoryDictionary()
                    .Build();
            ServerSettings.OutboundPeer =
                new ConfigurationBuilder<IInboundPeer>()
                    .UseInMemoryDictionary()
                    .Build();
        }
    }
}