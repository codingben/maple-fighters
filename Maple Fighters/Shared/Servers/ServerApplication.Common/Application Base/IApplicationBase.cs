using ServerCommunicationInterfaces;

namespace ServerApplication.Common.ApplicationBase
{
    public interface IApplicationBase
    {
        void Startup();
        void Shutdown();

        void OnConnected(IClientPeer clientPeer);
    }
}