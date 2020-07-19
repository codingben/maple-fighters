namespace Game.Application.Components
{
    public interface IWebSocketSessionCollection
    {
        bool Add(int id, WebSocketSessionData webSocketSessionData);

        bool Remove(int id);

        bool TryGet(int id, out WebSocketSessionData webSocketSessionData);
    }
}