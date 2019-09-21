using CommonCommunicationInterfaces;

namespace Scripts.Network.Core
{
    public interface IServiceBase
    {
        void Connect();

        void Disconnect();

        void SetNetworkTrafficState(NetworkTrafficState state);

        bool IsConnected();
    }
}