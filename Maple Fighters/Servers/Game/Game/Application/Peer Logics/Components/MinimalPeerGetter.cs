using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Game.Application.PeerLogic.Components
{
    internal class MinimalPeerGetter : Component<IPeerEntity>
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