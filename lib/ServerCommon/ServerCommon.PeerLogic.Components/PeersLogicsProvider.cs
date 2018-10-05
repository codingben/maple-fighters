using System;
using Common.ComponentModel;

namespace ServerCommon.PeerLogic.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class PeersLogicsProvider : ComponentBase, IPeersLogicsProvider
    {
        private readonly object locker = new object();
        private readonly PeersLogicsCollection<IPeerLogicBase> peersLogics;

        public PeersLogicsProvider()
        {
            peersLogics = new PeersLogicsCollection<IPeerLogicBase>();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            RemoveAllPeersLogics();
        }

        public void AddPeerLogic(int peerId, IPeerLogicBase peerLogic)
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
            Action<IPeerLogicBase> peerLogic)
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