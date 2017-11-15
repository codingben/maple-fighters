using CommonTools.Log;
using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    internal class MinimalPeerGetter : Component<IPeerEntity>, IMinimalPeerGetter
    {
        private readonly IMinimalPeer peer;

        public MinimalPeerGetter(IMinimalPeer peer)
        {
            this.peer = peer.AssertNotNull();
        }

        public IMinimalPeer GetPeer()
        {
            return peer;
        }
    }
}