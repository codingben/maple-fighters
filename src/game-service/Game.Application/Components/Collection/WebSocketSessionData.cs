using Fleck;

namespace Game.Application.Components
{
    public struct WebSocketSessionData
    {
        public int Id { get; }

        public IWebSocketConnection Connection { get; }

        public WebSocketSessionData(int id, IWebSocketConnection connection)
        {
            Id = id;
            Connection = connection;
        }
    }
}