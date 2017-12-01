using System.Collections.Generic;
using ComponentModel.Common;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public interface IPeerContainer : IExposableComponent
    {
        void AddPeerLogic(IClientPeerWrapper<IClientPeer> peerLogic);
        void DisconnectAllPeers();

        IClientPeerWrapper<IClientPeer> GetPeerWrapper(int peerId);
        IEnumerable<IClientPeerWrapper<IClientPeer>> GetAllPeerWrappers();

        int GetPeersCount();
    }
}