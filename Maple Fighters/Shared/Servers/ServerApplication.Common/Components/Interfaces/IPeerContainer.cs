using System.Collections.Generic;
using PeerLogic.Common;

namespace ServerApplication.Common.Components.Interfaces
{
    public interface IPeerContainer
    {
        void AddPeerLogic(IClientPeerWrapper peerLogic);

        IClientPeerWrapper GetPeerWrapper(int peerId);
        IEnumerable<IClientPeerWrapper> GetAllPeerWrappers();

        int GetPeersCount();
    }
}