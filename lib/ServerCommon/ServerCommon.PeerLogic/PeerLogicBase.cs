using System;
using Common.ComponentModel;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic
{
    /// <summary>
    /// A base implementation for the client peer logic.
    /// </summary>
    public abstract class PeerLogicBase : IPeerLogicBase, IDisposable
    {
        public IExposedComponentsProvider ExposedComponents =>
            Components.ProvideExposed();

        protected IClientPeer Peer { get; private set; }

        protected int PeerId { get; private set; }

        protected IComponentsProvider Components => new ComponentsProvider();

        /// <summary>
        /// Sets the client peer and peer id.
        /// </summary>
        /// <param name="peer">The client peer.</param>
        /// <param name="peerId">The client peer id.</param>
        public void Setup(IClientPeer peer, int peerId)
        {
            Peer = peer;
            PeerId = peerId;

            OnSetup();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IDisposable.Dispose" /> for more information.
        /// </summary>
        public void Dispose()
        {
            Components?.Dispose();

            OnCleanup();
        }

        /// <summary>
        /// Setups the peer logic.
        /// </summary>
        public abstract void OnSetup();

        /// <summary>
        /// Cleanups of the peer logic.
        /// </summary>
        public abstract void OnCleanup();
    }
}