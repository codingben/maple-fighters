using System;
using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common
{
    public interface IPeerLogicBase : IDisposable
    {
        IContainer Components { get; }

        void Initialize(IClientPeerWrapper<IClientPeer> peer);
    }
}