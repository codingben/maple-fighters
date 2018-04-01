using System.Collections.Generic;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public interface IPeerContainer
    {
        void AddPeerLogic(IClientPeerWrapper peerLogic);

        IClientPeerWrapper GetPeerWrapper(int peerId);
        IEnumerable<IClientPeerWrapper> GetAllPeerWrappers();

        int GetPeersCount();
    }
}