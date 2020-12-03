using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common.Components.Interfaces;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    internal class ClientPeerProvider : Component, IClientPeerProvider
    {
        public int PeerId { get; }
        public IClientPeer Peer { get; }

        public ClientPeerProvider(int peerId, IClientPeer peer)
        {
            PeerId = peerId;
            Peer = peer.AssertNotNull();
        }
    }
}