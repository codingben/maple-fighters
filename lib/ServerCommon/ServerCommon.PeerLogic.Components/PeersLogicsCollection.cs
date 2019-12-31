using System.Collections.Generic;

namespace ServerCommon.PeerLogic.Components
{
    internal class PeersLogicsCollection<TPeerLogic>
        where TPeerLogic : class
    {
        private readonly Dictionary<int, TPeerLogic> collection;

        public PeersLogicsCollection()
        {
            collection = new Dictionary<int, TPeerLogic>();
        }

        public void Add(int id, TPeerLogic peerLogic)
        {
            if (!collection.ContainsKey(id))
            {
                collection.Add(id, peerLogic);
            }
        }

        public void Remove(int id)
        {
            if (collection.ContainsKey(id))
            {
                collection.Remove(id);
            }
        }

        public TPeerLogic Get(int id)
        {
            TPeerLogic peerLogic = null;

            if (collection.ContainsKey(id))
            {
                peerLogic = collection[id];
            }

            return peerLogic;
        }

        public IEnumerable<TPeerLogic> GetAllLogics()
        {
            return collection.Values;
        }

        public void RemoveAll()
        {
            collection.Clear();
        }

        public int Count()
        {
            return collection.Count;
        }
    }
}