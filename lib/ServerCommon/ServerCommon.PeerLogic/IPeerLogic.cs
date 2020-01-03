using System;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic
{
    public interface IPeerLogic<in TPeer> : IDisposable
        where TPeer : class, IMinimalPeer
    {
        void Setup(TPeer peer, int peerId);
    }
}