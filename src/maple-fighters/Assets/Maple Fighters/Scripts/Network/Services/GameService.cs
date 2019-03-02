using Scripts.Network.APIs;
using Scripts.Network.Core;
using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.Network.Services
{
    public class GameService : ServiceBase, IGameService
    {
        public IAuthorizerApi GetAuthorizerApi() => authorizerApi;

        public ICharacterSelectorApi GetCharacterSelectorApi() => characterSelectorApi;

        public IGameSceneApi GetGameSceneApi() => gameSceneApi;

        private IAuthorizerApi authorizerApi;
        private ICharacterSelectorApi characterSelectorApi;
        private IGameSceneApi gameSceneApi;

        protected override void OnAwake()
        {
            base.OnAwake();

            DontDestroyOnLoad(gameObject);

            var connectionInformation = 
                ServerConfiguration.GetInstance()
                    .GetConnectionInformation(ServerType.Game);

            Connect(connectionInformation);
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            Disconnect();
        }

        protected override void OnConnected()
        {
            base.OnConnected();

            authorizerApi = new AuthorizerApi();
            authorizerApi.SetServerPeer(GetServerPeer());

            if (GameConfiguration.GetInstance().Environment
                == Environment.Production)
            {
                characterSelectorApi = new CharacterSelectorApi();
            }
            else
            {
                characterSelectorApi = new DummyCharacterSelectorApi();
            }

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
