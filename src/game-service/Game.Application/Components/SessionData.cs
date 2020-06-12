namespace Game.Application.Components
{
    public struct SessionData
    {
        public string Id { get; }

        public SessionData(string id)
        {
            Id = id;
        }
    }
}