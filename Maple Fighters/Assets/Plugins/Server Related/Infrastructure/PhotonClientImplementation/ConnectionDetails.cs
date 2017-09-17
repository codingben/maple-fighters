using ExitGames.Client.Photon;

namespace PhotonClientImplementation
{
    public struct ConnectionDetails
    {
        public ConnectionProtocol ConnectionProtocol { get; }
        public DebugLevel DebugLevel { get; }

        public ConnectionDetails(ConnectionProtocol connectionProtocol, DebugLevel debugLevel)
        {
            ConnectionProtocol = connectionProtocol;
            DebugLevel = debugLevel;
        }
    }
}