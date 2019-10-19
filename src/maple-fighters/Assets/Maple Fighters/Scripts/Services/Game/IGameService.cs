using Scripts.Services.Authorizer;

namespace Scripts.Services.Game
{
    public interface IGameService
    {
        IAuthorizerApi AuthorizerApi { get; }

        ICharacterSelectorApi CharacterSelectorApi { get; }

        IGameSceneApi GameSceneApi { get; }
    }
}