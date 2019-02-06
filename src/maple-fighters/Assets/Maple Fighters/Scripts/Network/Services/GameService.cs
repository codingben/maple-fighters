using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.Services
{
    public class GameService : ServiceBase, IGameService
    {
        public IAuthorizerApi GetAuthorizerApi() => authorizerApi;

        public ICharacterSelectorApi GetCharacterSelectorApi() => characterSelectorApi;

        public IGameSceneApi GetGameSceneApi() => gameSceneApi;

        private AuthorizerApi authorizerApi;
        private CharacterSelectorApi characterSelectorApi;
        private IGameSceneApi gameSceneApi;

        private void Awake()
        {
            var connectionInformation = 
                ServerConfiguration.GetInstance()
                    .GetConnectionInformation(ServerType.Game);

            Connect(connectionInformation);
        }

        protected override void OnConnected()
        {
            base.OnConnected();

            authorizerApi = new AuthorizerApi();
            authorizerApi.SetServerPeer(GetServerPeer());

            characterSelectorApi = new CharacterSelectorApi();
            characterSelectorApi.SetServerPeer(GetServerPeer());

            if (GameConfiguration.GetInstance().Environment
                == Environment.Production)
            {
                gameSceneApi = new GameSceneApi();
            }
            else
            {
                gameSceneApi = new DummyGameSceneApi();
            }

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
