namespace Game.Application.Components
{
    public interface IWebSocketSessionCollection
    {
        bool AddSessionData(int id, WebSocketSessionData webSocketSessionData);

        bool RemoveSessionData(int id);

        bool GetSessionData(int id, out WebSocketSessionData webSocketSessionData);
    }
}