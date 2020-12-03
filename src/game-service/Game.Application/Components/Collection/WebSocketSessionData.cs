namespace Game.Application.Components
{
    public struct WebSocketSessionData
    {
        public string Id { get; }

        public WebSocketSessionData(string id)
        {
            Id = id;
        }
    }
}