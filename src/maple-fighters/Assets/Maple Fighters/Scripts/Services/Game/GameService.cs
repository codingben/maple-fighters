using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using Network.Scripts;
using Network.Utils;
using Scripts.Services.Authorizer;

namespace Scripts.Services.Game
{
    public class GameService : Singleton<GameService>, IGameService
    {
        public IAuthorizerApi AuthorizerApi
        {
            get
            {
                if (authorizerApi == null)
                {
                    authorizerApi = new DummyAuthorizerApi(serverPeer);
                }

                return authorizerApi;
            }
        }

        public ICharacterSelectorApi CharacterSelectorApi
        {
            get
            {
                if (characterSelectorApi == null)
                {
                    characterSelectorApi =
                        new DummyCharacterSelectorApi(serverPeer);
                }

                return characterSelectorApi;
            }
        }

        public IGameSceneApi GameSceneApi
        {
            get
            {
                if (gameSceneApi == null)
                {
                    gameSceneApi = new DummyGameSceneApi(serverPeer);
                }

                return gameSceneApi;
            }
        }

        private IAuthorizerApi authorizerApi;
        private ICharacterSelectorApi characterSelectorApi;
        private IGameSceneApi gameSceneApi;
        private IServerPeer serverPeer;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            coroutinesExecutor.StartTask(ConnectAsync);

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            ((IDisposable)authorizerApi)?.Dispose();
            ((IDisposable)characterSelectorApi)?.Dispose();
            ((IDisposable)gameSceneApi)?.Dispose();
            coroutinesExecutor?.Dispose();
        }

        private async Task ConnectAsync(IYield yield)
        {
            var serverConnector = new DummyServerConnector();
            var connectionInfo = new PeerConnectionInformation();
            var connectionProtocol = ConnectionProtocol.Tcp;

            serverPeer =
                await serverConnector.Connect(
                    yield,
                    connectionInfo,
                    connectionProtocol);
        }

        public void SetNetworkTrafficState(
            NetworkTrafficState networkTrafficState)
        {
            if (serverPeer != null)
            {
                serverPeer.NetworkTrafficState = networkTrafficState;
            }
        }
    }
}