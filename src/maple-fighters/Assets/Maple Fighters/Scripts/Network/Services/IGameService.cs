namespace Scripts.Services
{
    public interface IGameService : IServiceBase
    {
        IAuthorizerApi AuthorizerApi { get; }

        ICharacterSelectorApi CharacterSelectorApi { get; }

        IGameSceneApi GameSceneApi { get; }
    }
}