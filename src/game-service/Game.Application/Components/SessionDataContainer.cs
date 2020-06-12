using System.Collections.Concurrent;
using Common.ComponentModel;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class SessionDataContainer : ComponentBase, ISessionDataContainer
    {
        private readonly ConcurrentDictionary<int, SessionData> container;

        public SessionDataContainer()
        {
            container = new ConcurrentDictionary<int, SessionData>();
        }

        public bool AddSessionData(int id, SessionData sessionData)
        {
            return container.TryAdd(id, sessionData);
        }

        public bool RemoveSessionData(int id)
        {
            return container.TryRemove(id, out _);
        }

        public bool GetSessionData(int id, out SessionData sessionData)
        {
            return container.TryGetValue(id, out sessionData);
        }
    }
}