using CommonCommunicationInterfaces;
using JetBrains.Annotations;
using Scripts.Services;

namespace Scripts.Utils
{
    [System.Serializable]
    public class ConnectionInformation
    {
        [UsedImplicitly] public string Name;
        [UsedImplicitly] public ServerType ServerType;
        [UsedImplicitly] public PeerConnectionInformation UdpConnectionDetails;
        [UsedImplicitly] public PeerConnectionInformation TcpConnectionDetails;
        [UsedImplicitly] public PeerConnectionInformation WebConnectionDetails;
    }
}