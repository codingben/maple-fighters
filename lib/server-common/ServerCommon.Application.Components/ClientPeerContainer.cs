using System.Collections.Generic;
using Common.ComponentModel;

namespace ServerCommon.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class ClientPeerContainer : ComponentBase, IClientPeerContainer
    {
        private readonly Dictionary<int, IPeerWrapper> collection;
        private readonly object locker = new object();

        private IOnClientPeerContainerRemoved onClientPeerContainerRemoved;

        public ClientPeerContainer()
        {
            collection = new Dictionary<int, IPeerWrapper>();
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            onClientPeerContainerRemoved =
                Components.Get<IOnClientPeerContainerRemoved>();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            lock (locker)
            {
                var peerWrappers = collection.Values;
                onClientPeerContainerRemoved.Handle(peerWrappers);
            }
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