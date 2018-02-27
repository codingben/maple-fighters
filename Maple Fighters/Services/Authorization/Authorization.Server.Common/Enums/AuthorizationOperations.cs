namespace Authorization.Server.Common
{
    public enum AuthorizationOperations : byte
    {
        CreateAuthorization,
        RemoveAuthorization,
        AccessTokenAuthorization,
        UserAuthorization
    }
}