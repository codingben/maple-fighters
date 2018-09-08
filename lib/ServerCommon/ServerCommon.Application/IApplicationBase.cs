using ServerCommunicationInterfaces;

namespace ServerCommon.Application
{
    public interface IApplicationBase
    {
        void Startup();

        void Shutdown();

        void Connected(IClientPeer clientPeer);
    }
}