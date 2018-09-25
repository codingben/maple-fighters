using ServerCommunicationInterfaces;

namespace ServerCommon.PeerBase
{
    /// <summary>
    /// Represents the client peer which will handle peer logic.
    /// </summary>
    public interface IPeerBase
    {
        /// <summary>
        /// Initializes the client peer.
        /// </summary>
        /// <param name="peer">The client peer.</param>
        void Connected(IClientPeer peer);
    }
}