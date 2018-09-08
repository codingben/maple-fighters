using System;
using System.Collections.Generic;

namespace ServerCommon.PeerLogic.Components
{
    public interface IPeersLogicProvider
    {
        void AddPeerLogic(IPeerLogicProvider peerLogic);

        void RemovePeerLogic(int peerId);

        void RemoveAllPeersLogic();

        IPeerLogicProvider ProvidePeerLogic(int peerId);

        void ProvideAllPeersLogic(
            Action<IEnumerable<IPeerLogicProvider>> peerLogicProviders);

        int CountPeers();
    }
}