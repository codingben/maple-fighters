using System;
using System.Collections.Generic;
using Common.ComponentModel;
using CommonTools.Log;
using ServerCommon.Application.Components;

namespace ServerCommon.PeerLogic.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class PeersLogicProvider : ComponentBase, IPeersLogicProvider
    {
        private readonly object locker = new object();
        private readonly PeerLogicsCollection<IPeerLogicProvider> peerLogics;

        private IIdGenerator idGenerator;

        public PeersLogicProvider()
        {
            peerLogics = new PeerLogicsCollection<IPeerLogicProvider>();
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            idGenerator = Components.Get<IIdGenerator>().AssertNotNull();
        }

        public void AddPeerLogic(IPeerLogicProvider peerLogic)
        {
            lock (locker)
            {
                var id = idGenerator.GenerateId();
                peerLogics.Add(id, peerLogic);
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
                    logic.RemovePeerLogic();
                }

                peerLogics.RemoveAll();
            }
        }

        public IPeerLogicProvider ProvidePeerLogic(int peerId)
        {
            lock (locker)
            {
                return peerLogics[peerId];
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