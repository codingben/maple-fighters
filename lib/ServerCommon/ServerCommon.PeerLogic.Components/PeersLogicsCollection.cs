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
                if (!peersLogics.TryGetValue(id, out var peerLogic))
                {
                    throw new PeerLogicException(
                        $"A peer with id {id} does not exist in a peers logic collection.");
                }

                return peerLogic;
            }
        }

        public void Add(int peerId, TPeerLogic peerLogic)
        {
            if (peersLogics.ContainsKey(peerId))
            {
                throw new PeerLogicException(
                    $"Failed to add a new peer with id {peerId} because it already exists.");
            }

            peersLogics.Add(peerId, peerLogic);
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