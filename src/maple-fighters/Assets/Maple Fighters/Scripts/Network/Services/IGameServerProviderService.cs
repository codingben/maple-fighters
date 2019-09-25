using Scripts.Network.APIs;

namespace Scripts.Network.Services
{
    public interface IGameServerProviderService
    {
        IAuthorizerApi GetAuthorizerApi();

        IGameServerProviderApi GetGameServerProviderApi();
    }
}