using ServerCommunicationInterfaces;

namespace ServerCommon.Application
{
    /// <summary>
    /// Creator of a new connected peer.
    /// </summary>
    public interface IClientPeerCreator
    {
        /// <summary>
        /// Uses a client peer and creates peer logic.
        /// </summary>
        /// <param name="peer">The new peer.</param>
        void Create(IClientPeer peer);
    }
}