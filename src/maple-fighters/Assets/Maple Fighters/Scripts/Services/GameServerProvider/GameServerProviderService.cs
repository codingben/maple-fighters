using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using Network.Scripts;
using Network.Utils;
using Scripts.Services.Authorizer;

namespace Scripts.Services.GameServerProvider
{
    public class GameServerProviderService : Singleton<GameServerProviderService>, IGameServerProviderService
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

        public IGameServerProviderApi GameServerProviderApi
        {
            get
            {
                if (gameServerProviderApi == null)
                {
                    gameServerProviderApi = new DummyGameServerProviderApi(serverPeer);
                }

                return gameServerProviderApi;
            }
        }

        private IAuthorizerApi authorizerApi;
        private IGameServerProviderApi gameServerProviderApi;
        private IServerPeer serverPeer;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            coroutinesExecutor.StartTask(ConnectAsync);
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            ((IDisposable)authorizerApi)?.Dispose();
            ((IDisposable)gameServerProviderApi)?.Dispose();
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
    }
}