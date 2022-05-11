using System.Collections.Concurrent;
using Game.Logger;

namespace Game.Application.Components
{
    public class WebSocketSessionCollection : ComponentBase, IWebSocketSessionCollection
    {
        private readonly ConcurrentDictionary<int, WebSocketSessionData> collection;

        public WebSocketSessionCollection()
        {
            collection = new ConcurrentDictionary<int, WebSocketSessionData>();
        }

        public bool Add(int id, WebSocketSessionData webSocketSessionData)
        {
            var isAdded = collection.TryAdd(id, webSocketSessionData);
            if (isAdded)
            {
                GameLogger.Debug($"Client #{id} is connected.");
            }

            return isAdded;
        }

        public bool Remove(int id)
        {
            var isRemoved = collection.TryRemove(id, out _);
            if (isRemoved)
            {
                GameLogger.Debug($"Client #{id} disconnected.");
            }

            return isRemoved;
        }

        public bool TryGet(int id, out WebSocketSessionData webSocketSessionData)
        {
            return collection.TryGetValue(id, out webSocketSessionData);
        }
    }
}