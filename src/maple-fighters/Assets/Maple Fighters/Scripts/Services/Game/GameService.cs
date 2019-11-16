using System;
using ClientCommunicationInterfaces;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using Network.Scripts;
using PhotonClientImplementation;
using ScriptableObjects.Configurations;
using Scripts.Services.Authorizer;

namespace Scripts.Services.Game
{
    public class GameService : NetworkService
    {
        public IAuthorizerApi AuthorizerApi { get; private set; }

        public ICharacterSelectorApi CharacterSelectorApi { get; private set; }

        public IGameSceneApi GameSceneApi { get; private set; }

        private IServerPeer gamePeer;

        private ExternalCoroutinesExecutor coroutinesExecutor;
        private GameServerInfoProvider gameServerInfoProvider;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            gameServerInfoProvider = FindObjectOfType<GameServerInfoProvider>();

            // TODO: Remove
            coroutinesExecutor.StartTask(ConnectAsync);

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDisable()
        {
            gamePeer?.Disconnect();
        }

        private void OnDestroy()
        {
            ((IDisposable)AuthorizerApi)?.Dispose();
            ((IDisposable)CharacterSelectorApi)?.Dispose();
            ((IDisposable)GameSceneApi)?.Dispose();

            coroutinesExecutor?.Dispose();
        }

        protected override void OnConnected(IServerPeer serverPeer)
        {
            gamePeer = serverPeer;

            var isDummy = NetworkConfiguration.GetInstance().IsDummy();
            if (isDummy)
            {
                AuthorizerApi = new DummyAuthorizerApi(serverPeer);
                CharacterSelectorApi = new DummyCharacterSelectorApi(serverPeer);
                GameSceneApi = new DummyGameSceneApi(serverPeer);
            }
            else
            {
                AuthorizerApi = new AuthorizerApi(serverPeer);
                CharacterSelectorApi = new CharacterSelectorApi(serverPeer);
                GameSceneApi = new GameSceneApi(serverPeer);
            }
        }

        protected override IServerConnector GetServerConnector()
        {
            IServerConnector serverConnector;

            var isDummy = NetworkConfiguration.GetInstance().IsDummy();
            if (isDummy)
            {
                serverConnector = new DummyServerConnector();
            }
            else
            {
                serverConnector =
                    new PhotonServerConnector(() => coroutinesExecutor);
            }

            return serverConnector;
        }

        protected override PeerConnectionInformation GetConnectionInfo()
        {
            return gameServerInfoProvider.GetConnectionInfo();
        }

        protected override ConnectionProtocol GetConnectionProtocol()
        {
            var serverInfo = NetworkConfiguration.GetInstance().GetServerInfo(ServerType.Game);
            var protocol = serverInfo.Protocol;

            return protocol;
        }
    }
}