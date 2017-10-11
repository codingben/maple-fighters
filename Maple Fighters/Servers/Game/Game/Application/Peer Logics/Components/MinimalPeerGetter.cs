using CommonTools.Log;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.PeerLogic;

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