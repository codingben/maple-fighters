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
                    throw new ServerSettingsException(
                        "The inbound peer configuration is not initialized.");
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
                    throw new ServerSettingsException(
                        "The outbound peer configuration is not initialized.");
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
                    throw new ServerSettingsException(
                        "The databases configuration is not initialized.");
                }

                return _databases;
            }

            set => _databases = value;
        }

        private static IDatabases _databases;
    }
}