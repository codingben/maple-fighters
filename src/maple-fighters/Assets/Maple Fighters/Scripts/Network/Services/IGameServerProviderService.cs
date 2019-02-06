namespace Scripts.Network
{
    public interface IGameServerProviderService : IServiceBase
    {
        IAuthorizerApi GetAuthorizerApi();

        IGameServerProviderApi GetGameServerProviderApi();
    }
}