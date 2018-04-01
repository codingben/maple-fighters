using System;

namespace PeerLogic.Common
{
    public interface IPeerLogicHandler : IDisposable
    {
        IPeerLogicBase PeerLogic { get; }

        void SetPeerLogic<TPeerLogic>(TPeerLogic peerLogic)
            where TPeerLogic : IPeerLogicBase;
    }
}