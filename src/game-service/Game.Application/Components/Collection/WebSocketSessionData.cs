namespace Game.Application.Components
{
    public struct WebSocketSessionData
    {
        public int Id { get; }

        public WebSocketSessionData(int id)
        {
            Id = id;
        }
    }
}