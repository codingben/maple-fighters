using System.Collections.Concurrent;
using Common.ComponentModel;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class SessionDataCollection : ComponentBase, ISessionDataCollection
    {
        private readonly ConcurrentDictionary<int, SessionData> collection;

        public SessionDataCollection()
        {
            collection = new ConcurrentDictionary<int, SessionData>();
        }

        public bool AddSessionData(int id, SessionData sessionData)
        {
            return collection.TryAdd(id, sessionData);
        }

        public bool RemoveSessionData(int id)
        {
            return collection.TryRemove(id, out _);
        }

        public bool GetSessionData(int id, out SessionData sessionData)
        {
            return collection.TryGetValue(id, out sessionData);
        }
    }
}