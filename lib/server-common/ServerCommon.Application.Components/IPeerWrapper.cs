using System;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application.Components
{
    public interface IPeerWrapper : IDisposable
    {
        IMinimalPeer Peer { get; }

        void ChangePeerLogic(IDisposable newPeerLogic);
    }
}