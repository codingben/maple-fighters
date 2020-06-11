using System.Collections.Concurrent;
using Common.ComponentModel;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class SessionIdContainer : ComponentBase, ISessionIdContainer
    {
        private readonly ConcurrentDictionary<int, string> container;

        public SessionIdContainer()
        {
            container = new ConcurrentDictionary<int, string>();
        }

        public bool AddSessionId(int id, string sessionId)
        {
            return container.TryAdd(id, sessionId);
        }

        public bool GetSessionId(int id, out string sessionId)
        {
            return container.TryGetValue(id, out sessionId);
        }

        public bool RemoveSessionId(int id)
        {
            return container.TryRemove(id, out _);
        }
    }
}