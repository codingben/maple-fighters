using ServerCommunicationInterfaces;

namespace ServerApplication.Common.ApplicationBase
{
    /// <summary>
    /// Every server application should inherit from it in order to make an initialization.
    /// </summary>
    public abstract class Application
    {
        public abstract void Initialize();

        public abstract void OnConnected(IClientPeer clientPeer);

        public abstract void Dispose();
    }
}