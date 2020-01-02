using ServerCommunicationInterfaces;

namespace ServerCommon.PeerBase
{
    /// <summary>
    /// A common peer base implementation.
    /// </summary>
    public interface IPeerBase
    {
        void Connected(IClientPeer peer, int peerId);
    }
}