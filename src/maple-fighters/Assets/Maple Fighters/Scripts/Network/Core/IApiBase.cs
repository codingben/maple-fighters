using System;
using CommonCommunicationInterfaces;

namespace Scripts.Network.Core
{
    public interface IApiBase : IDisposable
    {
        void SetServerPeer(IServerPeer serverPeer);
    }
}