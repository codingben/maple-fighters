using System;

namespace Scripts.Services
{
    public interface IPeerLogicBase : IDisposable
    {
        void Awake(IServerPeerHandler serverPeerHandler);
    }
}