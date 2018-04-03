namespace Authorization.Service.Application.Components.Interfaces
{
    internal interface IAccessTokenExistence
    {
        bool Exists(int userId);
        bool Exists(string accessToken);
    }
}