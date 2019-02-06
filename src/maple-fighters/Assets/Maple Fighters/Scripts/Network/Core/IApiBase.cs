using System;
using CommonCommunicationInterfaces;

namespace Scripts.Network
{
    public interface IApiBase : IDisposable
    {
        void SetServerPeer(IServerPeer serverPeer);
    }
}