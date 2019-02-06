using Scripts.Network.APIs;

namespace Scripts.Network
{
    public interface IGameServerProviderService : IServiceBase
    {
        IAuthorizerApi GetAuthorizerApi();

        IGameServerProviderApi GetGameServerProviderApi();
    }
}