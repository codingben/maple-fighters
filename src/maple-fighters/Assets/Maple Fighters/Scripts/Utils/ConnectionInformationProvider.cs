using System;
using CommonCommunicationInterfaces;
using Scripts.Services;

namespace Scripts.Utils
{
    [Serializable]
    public class ConnectionInformation
    {
        public string Name;
        public ServerType ServerType;
        public PeerConnectionInformation UdpConnectionDetails;
        public PeerConnectionInformation WebSocketConnectionDetails;
        public PeerConnectionInformation WebSocketSecureConnectionDetails;
    }
}