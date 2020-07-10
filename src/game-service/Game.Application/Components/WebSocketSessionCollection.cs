using System.Collections.Concurrent;
using Common.ComponentModel;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class WebSocketSessionCollection : ComponentBase, IWebSocketSessionCollection
    {
        private readonly ConcurrentDictionary<int, WebSocketSessionData> collection;

        public WebSocketSessionCollection()
        {
            collection = new ConcurrentDictionary<int, WebSocketSessionData>();
        }

        public bool AddSessionData(int id, WebSocketSessionData webSocketSessionData)
        {
            return collection.TryAdd(id, webSocketSessionData);
        }

        public bool RemoveSessionData(int id)
        {
            return collection.TryRemove(id, out _);
        }

        public bool GetSessionData(int id, out WebSocketSessionData webSocketSessionData)
        {
            return collection.TryGetValue(id, out webSocketSessionData);
        }
    }
}