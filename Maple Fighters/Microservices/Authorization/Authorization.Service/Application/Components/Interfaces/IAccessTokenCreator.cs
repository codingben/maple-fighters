namespace Authorization.Service.Application.Components.Interfaces
{
    internal interface IAccessTokenCreator
    {
        string Create(int userId);
    }
}