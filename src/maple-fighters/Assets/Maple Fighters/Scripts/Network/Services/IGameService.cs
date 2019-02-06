using Scripts.Network.APIs;
using Scripts.Network.Core;

namespace Scripts.Network
{
    public interface IGameService : IServiceBase
    {
        IAuthorizerApi GetAuthorizerApi();

        ICharacterSelectorApi GetCharacterSelectorApi();

        IGameSceneApi GetGameSceneApi();
    }
}