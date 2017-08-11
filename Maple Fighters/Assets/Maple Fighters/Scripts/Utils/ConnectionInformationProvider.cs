using CommonCommunicationInterfaces;
using JetBrains.Annotations;
using Scripts.Services;

namespace Scripts.Utils
{
    [System.Serializable]
    public class ConnectionInformation
    {
        [UsedImplicitly] public ServersType ServerType;
        [UsedImplicitly] public PeerConnectionInformation ConnectionDetails;
        [UsedImplicitly] public PeerConnectionInformation WebConnectionDetails;
    }
}