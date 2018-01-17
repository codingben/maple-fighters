using CommonTools.Log;
using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    internal class MinimalPeerGetter : Component, IMinimalPeerGetter
    {
        private readonly int peerId;
        private readonly IMinimalPeer peer;

        public MinimalPeerGetter(int peerId, IMinimalPeer peer)
        {
            this.peerId = peerId;
            this.peer = peer.AssertNotNull();
        }

        public int GetPeerId()
        {
            return peerId;
        }

        public IMinimalPeer GetPeer()
        {
            return peer;
        }
    }
}