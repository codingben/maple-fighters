using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common.Components.Interfaces;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    internal class MinimalPeerGetter : Component, IMinimalPeerGetter
    {
        public int PeerId { get; }
        public IMinimalPeer Peer { get; }

        public MinimalPeerGetter(int peerId, IMinimalPeer peer)
        {
            PeerId = peerId;
            Peer = peer.AssertNotNull();
        }
    }
}