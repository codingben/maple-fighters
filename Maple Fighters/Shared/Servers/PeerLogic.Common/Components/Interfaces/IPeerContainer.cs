using System.Collections.Generic;

namespace PeerLogic.Common.Components.Interfaces
{
    public interface IPeerContainer
    {
        void AddPeerLogic(IClientPeerWrapper peerLogic);

        IClientPeerWrapper GetPeerWrapper(int peerId);
        IEnumerable<IClientPeerWrapper> GetAllPeerWrappers();

        int GetPeersCount();
    }
}