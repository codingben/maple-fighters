using UnityEngine;

namespace Scripts.Services
{
    public class GameService : ServiceBase, IGameService
    {
        public IAuthorizerApi AuthorizerApi => authorizerApi;

        public ICharacterSelectorApi CharacterSelectorApi => characterSelectorApi;

        public IGameSceneApi GameSceneApi => gameSceneApi;

        private AuthorizerApi authorizerApi;
        private CharacterSelectorApi characterSelectorApi;
        private GameSceneApi gameSceneApi;

        protected override void OnConnected()
        {
            base.OnConnected();

            authorizerApi = new AuthorizerApi();
            authorizerApi.SetServerPeer(GetServerPeer());

            characterSelectorApi = new CharacterSelectorApi();
            characterSelectorApi.SetServerPeer(GetServerPeer());

            gameSceneApi = new GameSceneApi();
            gameSceneApi.SetServerPeer(GetServerPeer());

            Debug.Log("Connected to the game server.");
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            authorizerApi.Dispose();
            characterSelectorApi.Dispose();
            gameSceneApi.Dispose();

            Debug.Log("Disconnected from the game server.");
        }
    }
}
