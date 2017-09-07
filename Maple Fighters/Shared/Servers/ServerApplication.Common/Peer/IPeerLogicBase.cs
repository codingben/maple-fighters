using System;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public interface IPeerLogicBase : IDisposable
    {
        // IPeerEntity Entity { get; }

        void Initialize(IClientPeerWrapper<IClientPeer> peer);
    }
}