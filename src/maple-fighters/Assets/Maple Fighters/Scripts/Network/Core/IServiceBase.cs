using CommonCommunicationInterfaces;

namespace Scripts.Network
{
    public interface IServiceBase
    {
        void Connect(ConnectionInformation connectionInformation);

        void SetNetworkTrafficState(NetworkTrafficState state);

        void Disconnect();

        bool IsConnected();
    }
}