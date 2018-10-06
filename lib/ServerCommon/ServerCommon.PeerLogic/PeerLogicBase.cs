using System;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic
{
    /// <summary>
    /// A base implementation for the client peer logic.
    /// </summary>
    /// <typeparam name="TPeer">The peer.</typeparam>
    public abstract class PeerLogicBase<TPeer> : IPeerLogicBase<TPeer>
        where TPeer : class, IMinimalPeer
    {
        protected TPeer Peer { get; private set; }

        protected int PeerId { get; private set; }

        /// <summary>
        /// Sets the peer and peer id.
        /// </summary>
        /// <param name="peer">The peer.</param>
        /// <param name="peerId">The peer id.</param>
        public void Setup(TPeer peer, int peerId)
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