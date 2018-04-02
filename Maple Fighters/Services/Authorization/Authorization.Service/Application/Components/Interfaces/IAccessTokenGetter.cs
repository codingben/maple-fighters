namespace Authorization.Service.Application.Components.Interfaces
{
    internal interface IAccessTokenGetter
    {
        string Get(int userId);
        int? Get(string accessToken);
    }
}