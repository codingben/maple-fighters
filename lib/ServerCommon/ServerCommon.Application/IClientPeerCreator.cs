using ServerCommunicationInterfaces;

namespace ServerCommon.Application
{
    public interface IClientPeerCreator
    {
        void Create(IClientPeer clientPeer);
    }
}