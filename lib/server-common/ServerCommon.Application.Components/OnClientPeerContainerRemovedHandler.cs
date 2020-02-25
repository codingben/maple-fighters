using System.Collections.Generic;
using Common.ComponentModel;

namespace ServerCommon.Application.Components
{
    public class OnClientPeerContainerRemovedHandler : ComponentBase,
                                                       IOnClientPeerContainerRemoved
    {
        public void Handle(IEnumerable<IPeerWrapper> peerWrappers)
        {
            foreach (var peerWrapper in peerWrappers)
            {
                var peer = peerWrapper.Peer;
                if (peer.IsConnected)
                {
                    peer.Disconnect();
                }
            }
        }
    }
}