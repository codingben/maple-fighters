using System;
using Common.ComponentModel;

namespace ServerCommon.PeerLogic.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class InboundPeersLogicsProvider : ComponentBase,
                                              IPeersLogicsProvider
    {
        private readonly object locker = new object();
        private readonly PeersLogicsCollection<IInboundPeerLogicBase> peersLogics;

        public InboundPeersLogicsProvider()
        {
            peersLogics = new PeersLogicsCollection<IInboundPeerLogicBase>();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            RemoveAllPeersLogics();
        }

        public void AddPeerLogic(
            int peerId,
            IInboundPeerLogicBase peerLogic)
        {
            lock (locker)
            {
                peersLogics.Add(peerId, peerLogic);
            }
        }

        public void RemovePeerLogic(int peerId)
        {
            lock (locker)
            {
                peersLogics.Remove(peerId);
            }
        }

        public void RemoveAllPeersLogics()
        {
            lock (locker)
            {
                peersLogics.RemoveAll();
            }
        }

        public void ProvidePeerLogic(
            int peerId,
            Action<IInboundPeerLogicBase> peerLogic)
        {
            lock (locker)
            {
                var logic = peersLogics[peerId];
                peerLogic.Invoke(logic);
            }
        }

        public int CountPeers()
        {
            lock (locker)
            {
                return peersLogics.Count();
            }
        }
    }
}