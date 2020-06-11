namespace Game.Application.Components
{
    public interface ISessionIdContainer
    {
        bool AddSessionId(int id, string sessionId);

        bool RemoveSessionId(int id);

        bool GetSessionId(int id, out string sessionId);
    }
}