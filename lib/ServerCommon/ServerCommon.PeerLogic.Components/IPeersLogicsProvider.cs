using System;

namespace ServerCommon.PeerLogic.Components
{
    /// <summary>
    /// A place to keep all the peers logic in one place.
    /// </summary>
    public interface IPeersLogicsProvider
    {
        void AddPeerLogic(int peerId, IInboundPeerLogicBase peerLogic);

        void RemovePeerLogic(int peerId);

        void ProvidePeerLogic(int peerId, Action<IInboundPeerLogicBase> peerLogic);

        int CountPeers();
    }
}