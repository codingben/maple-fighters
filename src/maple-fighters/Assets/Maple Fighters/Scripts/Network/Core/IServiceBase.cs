using CommonCommunicationInterfaces;

namespace Scripts.Network.Core
{
    public interface IServiceBase
    {
        void Connect(ConnectionInformation connectionInformation);

        void SetNetworkTrafficState(NetworkTrafficState state);

        void Disconnect();

        bool IsConnected();
    }
}