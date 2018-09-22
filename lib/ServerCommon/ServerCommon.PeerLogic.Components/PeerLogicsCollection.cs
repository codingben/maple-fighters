using System.Collections.Generic;

namespace ServerCommon.PeerLogic.Components
{
    internal class PeerLogicsCollection<TPeerLogic>
        where TPeerLogic : class
    {
        private readonly Dictionary<int, TPeerLogic> peerLogics;

        public PeerLogicsCollection()
        {
            peerLogics = new Dictionary<int, TPeerLogic>();
        }

        public TPeerLogic this[int id] 
        {
            get
            {
                if (!peerLogics.TryGetValue(id, out var peerLogic))
                {
                    throw new PeerLogicException(
                        $"A peer with id {id} does not exist in a peers logic collection.");
                }

                return peerLogic;
            }
        }

        public void Add(int peerId, TPeerLogic peerLogic)
        {
            if (peerLogics.ContainsKey(peerId))
            {
                throw new PeerLogicException(
                    $"Failed to add a new peer with id {peerId} because it already exists.");
            }

            peerLogics.Add(peerId, peerLogic);
        }

        public void Remove(int peerId)
        {
            if (!peerLogics.ContainsKey(peerId))
            {
                throw new PeerLogicException(
                    $"Failed to remove a peer with id {peerId} because it does not exist.");
            }

            peerLogics.Remove(peerId);
        }

        public void RemoveAll()
        {
            peerLogics.Clear();
        }

        public IEnumerable<TPeerLogic> GetAll()
        {
            return peerLogics.Values;
        }

        public int Count()
        {
            return peerLogics.Count;
        }
    }
}