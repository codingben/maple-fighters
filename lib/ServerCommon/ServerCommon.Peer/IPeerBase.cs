using System;
using ServerCommunicationInterfaces;

namespace ServerCommon.Peer
{
    public interface IPeerBase<out TPeer> : IDisposable
        where TPeer : class, IMinimalPeer
    {
        TPeer Peer { get; }

        int PeerId { get; }
    }
}