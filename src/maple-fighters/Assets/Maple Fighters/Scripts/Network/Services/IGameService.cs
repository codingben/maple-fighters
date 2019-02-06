using Scripts.Network.APIs;

namespace Scripts.Network
{
    public interface IGameService : IServiceBase
    {
        IAuthorizerApi GetAuthorizerApi();

        ICharacterSelectorApi GetCharacterSelectorApi();

        IGameSceneApi GetGameSceneApi();
    }
}