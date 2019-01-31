using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    public struct ServerConnectionInformation
    {
        public ServerType ServerType;
        public PeerConnectionInformation PeerConnectionInformation;

        public ServerConnectionInformation(
            ServerType serverType,
            PeerConnectionInformation peerConnectionInformation)
        {
            ServerType = serverType;
            PeerConnectionInformation = peerConnectionInformation;
        }
    }
}