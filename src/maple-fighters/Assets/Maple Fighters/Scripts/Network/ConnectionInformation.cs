using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    public struct ConnectionInformation
    {
        public ServerType ServerType;
        public PeerConnectionInformation PeerConnectionInformation;

        public ConnectionInformation(
            ServerType serverType,
            PeerConnectionInformation peerConnectionInformation)
        {
            ServerType = serverType;
            PeerConnectionInformation = peerConnectionInformation;
        }
    }
}