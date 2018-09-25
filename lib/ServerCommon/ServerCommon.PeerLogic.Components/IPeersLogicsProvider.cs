using System;

namespace ServerCommon.PeerLogic.Components
{
    /// <summary>
    /// A place to keep all the peers logic in one place.
    /// </summary>
    public interface IPeersLogicsProvider
    {
        void AddPeerLogic(int peerId, IPeerLogicBase peerLogic);

        void RemovePeerLogic(int peerId);

        void ProvidePeerLogic(
            int peerId,
            Action<IPeerLogicBase> peerLogic);

        int CountPeers();
    }
}