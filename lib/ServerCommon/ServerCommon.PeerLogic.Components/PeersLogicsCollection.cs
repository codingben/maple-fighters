using System.Collections.Generic;

namespace ServerCommon.PeerLogic.Components
{
    internal class PeersLogicsCollection<TPeerLogic>
        where TPeerLogic : class
    {
        private readonly Dictionary<int, TPeerLogic> peersLogics;

        public PeersLogicsCollection()
        {
            peersLogics = new Dictionary<int, TPeerLogic>();
        }

        public TPeerLogic this[int id] 
        {
            get
            {
                TPeerLogic peerLogic = null;

                if (peersLogics.ContainsKey(id))
                {
                    peerLogic = peersLogics[id];
                }

                return peerLogic;
            }
        }

        public void Add(int peerId, TPeerLogic peerLogic)
        {
            if (!peersLogics.ContainsKey(peerId))
            {
                peersLogics.Add(peerId, peerLogic);
            }
        }

        public void Remove(int peerId)
        {
            if (peersLogics.ContainsKey(peerId))
            {
                peersLogics.Remove(peerId);
            }
        }

        public IEnumerable<TPeerLogic> GetAllLogics()
        {
            return peersLogics.Values;
        }

        public void RemoveAll()
        {
            peersLogics.Clear();
        }

        public int Count()
        {
            return peersLogics.Count;
        }
    }
}