using ServerCommunicationInterfaces;

namespace ServerApplication.Common.ApplicationBase
{
    public interface IApplication
    {
        void Startup();
        void Shutdown();

        void OnConnected(IClientPeer clientPeer);
    }
}