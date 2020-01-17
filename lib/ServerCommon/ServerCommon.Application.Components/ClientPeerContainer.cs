using System;
using System.Collections.Generic;
using Common.ComponentModel;

namespace ServerCommon.Application.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class ClientPeerContainer : ComponentBase, IClientPeerContainer
    {
        private readonly Dictionary<int, IDisposable> collection;

        public ClientPeerContainer()
        {
            collection = new Dictionary<int, IDisposable>();
        }

        public void Add(int id, IDisposable peer)
        {
            collection.Add(id, peer);
        }

        public void Remove(int id)
        {
            collection.Remove(id);
        }

        public bool Get(int id, out IDisposable peer)
        {
            return collection.TryGetValue(id, out peer);
        }
    }
}