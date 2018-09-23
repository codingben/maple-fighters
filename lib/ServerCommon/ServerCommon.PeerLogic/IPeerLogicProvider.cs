namespace ServerCommon.PeerLogic
{
    /// <summary>
    /// Handles the peer logic of the client peer.
    /// </summary>
    public interface IPeerLogicProvider
    {
        /// <summary>
        /// Sets a new peer logic to the client peer.
        /// </summary>
        /// <typeparam name="TPeerLogic">The new peer logic.</typeparam>
        /// <param name="peerLogic">The desired peer logic.</param>
        void SetPeerLogic<TPeerLogic>(TPeerLogic peerLogic)
            where TPeerLogic : IPeerLogicBase;

        /// <summary>
        /// Unsets the peer logic from the client peer.
        /// </summary>
        void UnsetPeerLogic();

        /// <summary>
        /// Gets the peer logic of the client peer.
        /// </summary>
        /// <returns>The peer logic.</returns>
        IPeerLogicBase ProvidePeerLogic();
    }
}