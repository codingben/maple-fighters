using ServerCommon.Configuration.Definitions;

namespace ServerCommon.Configuration
{
    public static class ServerSettings
    {
        public static IInboundPeer InboundPeer
        {
            get
            {
                if (_inboundPeer == null)
                {
                    throw new ConfigurationNotInitializedException("Inbound Peer");
                }

                return _inboundPeer;
            }

            set => _inboundPeer = value;
        }

        private static IInboundPeer _inboundPeer;

        public static IInboundPeer OutboundPeer
        {
            get
            {
                if (_inboundPeer == null)
                {
                    throw new ConfigurationNotInitializedException("Outbound Peer");
                }

                return _outboundPeer;
            }

            set => _outboundPeer = value;
        }

        private static IInboundPeer _outboundPeer;

        public static IDatabases Databases
        {
            get
            {
                if (_databases == null)
                {
                    throw new ConfigurationNotInitializedException("Databases");
                }

                return _databases;
            }

            set => _databases = value;
        }

        private static IDatabases _databases;
    }
}