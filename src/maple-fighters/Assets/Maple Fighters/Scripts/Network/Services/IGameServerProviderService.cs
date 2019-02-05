namespace Scripts.Services
{
    public interface IGameServerProviderService : IServiceBase
    {
        IAuthorizerApi AuthorizerApi { get; }

        IGameServerProviderApi GameServerProviderApi { get; }
    }
}