using System;
using Common.ComponentModel;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic
{
    public class PeerLogicBase<TPeer> : IPeerLogicBase, IDisposable
        where TPeer : IMinimalPeer
    {
        public IExposedComponentsProvider ExposedComponents =>
            Components.ProvideExposed();

        protected TPeer Peer { get; private set; }
        protected int PeerId { get; private set; }

        protected IComponentsProvider Components => new ComponentsProvider();

        public void Initialize(TPeer peer, int peerId)
        {
            Peer = peer;
            PeerId = peerId;

            OnInitialized();
        }

        public void Dispose()
        {
            OnDispose();
        }

        public virtual void OnInitialized()
        {
            // Left blank intentionally
        }

        public virtual void OnDispose()
        {
            Components?.Dispose();
        }
    }
}