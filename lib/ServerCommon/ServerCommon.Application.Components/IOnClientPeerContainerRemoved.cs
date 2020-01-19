using System.Collections.Generic;

namespace ServerCommon.Application.Components
{
    public interface IOnClientPeerContainerRemoved
    {
        void Handle(IEnumerable<IPeerWrapper> peerWrappers);
    }
}