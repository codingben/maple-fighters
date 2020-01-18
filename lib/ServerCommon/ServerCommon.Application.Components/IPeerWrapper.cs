using System;

namespace ServerCommon.Application.Components
{
    public interface IPeerWrapper : IDisposable
    {
        void ChangePeerLogic(IDisposable newPeerLogic);
    }
}