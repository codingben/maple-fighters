namespace Game.Application.Components
{
    public interface ISessionDataContainer
    {
        bool AddSessionData(int id, SessionData sessionData);

        bool RemoveSessionData(int id);

        bool GetSessionData(int id, out SessionData sessionData);
    }
}