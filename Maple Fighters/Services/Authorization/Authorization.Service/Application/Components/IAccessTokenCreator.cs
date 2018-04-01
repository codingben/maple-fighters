namespace Authorization.Service.Application.Components
{
    internal interface IAccessTokenCreator
    {
        string Create(int userId);
    }
}