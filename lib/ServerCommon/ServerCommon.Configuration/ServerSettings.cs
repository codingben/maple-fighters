using ServerCommon.Configuration.Definitions;

namespace ServerCommon.Configuration
{
    public static class ServerSettings
    {
        public static IPeer Peer
        {
            get
            {
                if (_peer == null)
                {
                    throw new ServerSettingsException(
                        "The peer configuration is not initialized.");
                }

                return _peer;
            }

            set => _peer = value;
        }

        private static IPeer _peer;
    }
}