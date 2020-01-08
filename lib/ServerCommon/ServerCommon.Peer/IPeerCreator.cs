using ServerCommunicationInterfaces;

namespace ServerCommon.Peer
{
    public interface IPeerCreator<in TPeer>
        where TPeer : class, IMinimalPeer
    {
        void Create(TPeer peer);
    }
}