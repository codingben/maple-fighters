using System.Collections.Generic;
using Common.ComponentModel;

namespace ServerCommon.Application.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class ClientPeerContainer : ComponentBase, IClientPeerContainer
    {
        private readonly Dictionary<int, IPeerWrapper> collection;

        public ClientPeerContainer()
        {
            collection = new Dictionary<int, IPeerWrapper>();
        }

        public void Add(int id, IPeerWrapper peer)
        {
            collection.Add(id, peer);
        }

        public void Remove(int id)
        {
            collection.Remove(id);
        }

        public bool Get(int id, out IPeerWrapper peer)
        {
            return collection.TryGetValue(id, out peer);
        }
    }
}