namespace Scripts.Services
{
    public class GameService : ServiceBase, IGameService
    {
        public IAuthorizerApi AuthorizerApi { get; } = new AuthorizerApi();

        public ICharacterSelectorApi CharacterSelectorApi { get; } = new CharacterSelectorApi();

        public IGameSceneApi GameSceneApi { get; } = new GameSceneApi();

        protected override void OnConnected()
        {
            base.OnConnected();

            AuthorizerApi.SetServerPeer(GetServerPeer());
            CharacterSelectorApi.SetServerPeer(GetServerPeer());
            GameSceneApi.SetServerPeer(GetServerPeer());
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            AuthorizerApi.Dispose();
            CharacterSelectorApi.Dispose();
            GameSceneApi.Dispose();
        }
    }
}
