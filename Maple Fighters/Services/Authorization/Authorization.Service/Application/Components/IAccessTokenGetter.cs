namespace Authorization.Service.Application.Components
{
    internal interface IAccessTokenGetter
    {
        string Get(int userId);
        int? Get(string accessToken);
    }
}