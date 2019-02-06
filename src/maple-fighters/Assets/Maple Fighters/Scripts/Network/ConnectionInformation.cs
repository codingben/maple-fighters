using System;
using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    [Serializable]
    public struct ConnectionInformation
    {
        public string Name;
        public ServerType ServerType;
        public PeerConnectionInformation PeerConnectionInformation;

        public ConnectionInformation(
            ServerType serverType,
            PeerConnectionInformation peerConnectionInformation)
        {
            Name = string.Empty;
            ServerType = serverType;
            PeerConnectionInformation = peerConnectionInformation;
        }
    }
}