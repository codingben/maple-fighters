using Scripts.Services.Authorizer;

namespace Scripts.Services.GameServerProvider
{
    public interface IGameServerProviderService
    {
        IAuthorizerApi AuthorizerApi { get; }

        IGameServerProviderApi GameServerProviderApi { get; }
    }
}