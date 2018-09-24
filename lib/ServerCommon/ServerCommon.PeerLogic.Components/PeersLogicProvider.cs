using System;
using System.Collections.Generic;
using Common.ComponentModel;

namespace ServerCommon.PeerLogic.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class PeersLogicProvider : ComponentBase, IPeersLogicProvider
    {
        private readonly object locker = new object();
        private readonly PeerLogicsCollection<IPeerLogicProvider> peerLogics;

        public PeersLogicProvider()
        {
            peerLogics = new PeerLogicsCollection<IPeerLogicProvider>();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            RemoveAllPeersLogic();
        }

        public void AddPeerLogic(int peerId, IPeerLogicProvider peerLogic)
        {
            lock (locker)
            {
                peerLogics.Add(peerId, peerLogic);
            }
        }

        public void RemovePeerLogic(int peerId)
        {
            lock (locker)
            {
                peerLogics.Remove(peerId);
            }
        }

        public void RemoveAllPeersLogic()
        {
            lock (locker)
            {
                var logics = peerLogics.GetAll();
                foreach (var logic in logics)
                {
                    logic.UnsetPeerLogic();
                }

                peerLogics.RemoveAll();
            }
        }

        public void ProvidePeerLogic(
            int peerId,
            Action<IPeerLogicProvider> peerLogicProvider)
        {
            lock (locker)
            {
                var logic = peerLogics[peerId];
                peerLogicProvider.Invoke(logic);
            }
        }

        public void ProvideAllPeersLogic(
            Action<IEnumerable<IPeerLogicProvider>> peerLogicProviders)
        {
            lock (locker)
            {
                var logics = peerLogics.GetAll();
                peerLogicProviders.Invoke(logics);
            }
        }

        public int CountPeers()
        {
            lock (locker)
            {
                return peerLogics.Count();
            }
        }
    }
}