using System;

namespace ServerCommunication.Common
{
    public interface IOutboundServerPeerLogicBase : IDisposable
    {
        void Initialize();
    }
}