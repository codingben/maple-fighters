using Scripts.Network.APIs;
using Scripts.Network.Core;

namespace Scripts.Network.Services
{
    public interface IGameService : IServiceBase
    {
        IAuthorizerApi GetAuthorizerApi();

        ICharacterSelectorApi GetCharacterSelectorApi();

        IGameSceneApi GetGameSceneApi();
    }
}