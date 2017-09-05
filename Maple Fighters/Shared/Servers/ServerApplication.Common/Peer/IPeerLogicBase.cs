using System;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public interface IPeerLogicBase : IDisposable
    {
        IEntity Entity { get; }

        void Initialize(IClientPeerWrapper<IClientPeer> peer, int peerId);
    }
}