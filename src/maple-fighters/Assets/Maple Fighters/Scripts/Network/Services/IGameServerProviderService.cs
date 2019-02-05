namespace Scripts.Services
{
    public interface IGameServerProviderService : IServiceBase
    {
        IAuthorizerApi GetAuthorizerApi();

        IGameServerProviderApi GetGameServerProviderApi();
    }
}