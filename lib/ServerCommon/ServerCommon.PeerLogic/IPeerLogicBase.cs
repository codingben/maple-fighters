using System;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic
{
    public interface IPeerLogicBase<in TPeer> : IDisposable
        where TPeer : class, IMinimalPeer
    {
        void Setup(TPeer peer, int peerId);
    }
}