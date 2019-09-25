using Scripts.Network.APIs;

namespace Scripts.Network.Services
{
    public interface IGameService
    {
        IAuthorizerApi GetAuthorizerApi();

        ICharacterSelectorApi GetCharacterSelectorApi();

        IGameSceneApi GetGameSceneApi();
    }
}