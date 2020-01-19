using System.Collections.Generic;
using Common.ComponentModel;

namespace ServerCommon.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class ClientPeerContainer : ComponentBase, IClientPeerContainer
    {
        private readonly Dictionary<int, IPeerWrapper> collection;
        private readonly object locker = new object();

        public ClientPeerContainer()
        {
            collection = new Dictionary<int, IPeerWrapper>();
        }

        public void Add(int id, IPeerWrapper peer)
        {
            lock (locker)
            {
                collection.Add(id, peer);
            }
        }

        public void Remove(int id)
        {
            lock (locker)
            {
                collection.Remove(id);
            }
        }

        public bool Get(int id, out IPeerWrapper peer)
        {
            lock (locker)
            {
                return collection.TryGetValue(id, out peer);
            }
        }
    }
}