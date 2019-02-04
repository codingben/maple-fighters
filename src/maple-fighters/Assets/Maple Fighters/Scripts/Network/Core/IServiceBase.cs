using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    public interface IServiceBase
    {
        void Connect(ConnectionInformation connectionInformation);

        void SetNetworkTrafficState(NetworkTrafficState state);

        void Disconnect();

        bool IsConnected();
    }
}