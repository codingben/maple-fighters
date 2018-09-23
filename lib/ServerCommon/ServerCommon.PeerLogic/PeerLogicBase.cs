using System;
using Common.ComponentModel;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic
{
    /// <summary>
    /// A base implementation for the client peer logic.
    /// </summary>
    /// <typeparam name="TPeer">The client peer.</typeparam>
    public class PeerLogicBase<TPeer> : IPeerLogicBase, IDisposable
        where TPeer : IMinimalPeer
    {
        public IExposedComponentsProvider ExposedComponents =>
            Components.ProvideExposed();

        protected TPeer Peer { get; private set; }

        protected int PeerId { get; private set; }

        protected IComponentsProvider Components { get; private set; }

        /// <summary>
        /// Sets the client peer and peer id
        /// </summary>
        /// <param name="peer">The client peer.</param>
        /// <param name="peerId">The client peer id.</param>
        public void SetPeer(TPeer peer, int peerId)
        {
            Peer = peer;
            PeerId = peerId;

            Setup();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IDisposable.Dispose" /> for more information.
        /// </summary>
        public void Dispose()
        {
            Cleanup();
        }

        protected internal virtual void Setup()
        {
            Components = new ComponentsProvider();
        }

        protected internal virtual void Cleanup()
        {
            Components?.Dispose();
        }
    }
}