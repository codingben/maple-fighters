using System;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common
{
    public interface IPeerLogicBase : IDisposable
    {
        IPeerEntity Entity { get; }

        void Initialize(IClientPeerWrapper<IClientPeer> peer);
    }
}