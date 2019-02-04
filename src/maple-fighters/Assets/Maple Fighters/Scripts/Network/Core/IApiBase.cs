using System;
using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    public interface IApiBase : IDisposable
    {
        void SetServerPeer(IServerPeer serverPeer);
    }
}