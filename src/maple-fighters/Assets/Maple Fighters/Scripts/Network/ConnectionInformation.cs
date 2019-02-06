using System;
using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    [Serializable]
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