namespace Authorization.Server.Common
{
    public enum ServerOperations : byte
    {
        CreateAuthorization,
        RemoveAuthorization,
        AccessTokenAuthorization,
        UserAuthorization
    }
}