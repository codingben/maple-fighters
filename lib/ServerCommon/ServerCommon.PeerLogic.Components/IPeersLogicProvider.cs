using System;
using System.Collections.Generic;

namespace ServerCommon.PeerLogic.Components
{
    /// <summary>
    /// A place to keep all the peers logic in one place.
    /// </summary>
    public interface IPeersLogicProvider
    {
        void AddPeerLogic(int peerId, IPeerLogicProvider peerLogic);

        void RemovePeerLogic(int peerId);

        void ProvidePeerLogic(
            int peerId,
            Action<IPeerLogicProvider> peerLogicProvider);

        void ProvideAllPeersLogic(
            Action<IEnumerable<IPeerLogicProvider>> peerLogicProviders);

        int CountPeers();
    }
}