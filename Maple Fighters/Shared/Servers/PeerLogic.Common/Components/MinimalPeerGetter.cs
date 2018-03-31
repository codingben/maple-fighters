using CommonTools.Log;
using ComponentModel.Common;
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