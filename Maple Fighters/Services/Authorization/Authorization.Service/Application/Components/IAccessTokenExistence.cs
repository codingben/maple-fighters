namespace Authorization.Service.Application.Components
{
    internal interface IAccessTokenExistence
    {
        bool Exists(int userId);
        bool Exists(string accessToken);
    }
}