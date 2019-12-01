using System;
using ClientCommunicationInterfaces;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using Network.Scripts;
using PhotonClientImplementation;
using ScriptableObjects.Configurations;

namespace Scripts.Services.Game
{
    public class GameService : NetworkService
    {
        public ICharacterSelectorApi CharacterSelectorApi { get; private set; }

        public IGameSceneApi GameSceneApi { get; private set; }

        public IPeerDisconnectionNotifier DisconnectionNotifier =>
            gamePeer?.PeerDisconnectionNotifier;

        public bool IsConnected => gamePeer != null && gamePeer.IsConnected;

        public event Action Connected;

        private IServerPeer gamePeer;

        private ExternalCoroutinesExecutor coroutinesExecutor;
        private GameServerInfoProvider gameServerInfoProvider;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            gameServerInfoProvider = FindObjectOfType<GameServerInfoProvider>();

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
                CharacterSelectorApi =
                    new DummyCharacterSelectorApi(serverPeer);
                GameSceneApi = new DummyGameSceneApi(serverPeer);
            }
            else
            {
                CharacterSelectorApi = new CharacterSelectorApi(serverPeer);
                GameSceneApi = new GameSceneApi(serverPeer);
            }

            Connected?.Invoke();
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
            var connectionInformation = new PeerConnectionInformation();

            if (gameServerInfoProvider != null)
            {
                connectionInformation =
                    gameServerInfoProvider.GetConnectionInfo();
            }

            return connectionInformation;
        }

        protected override ConnectionProtocol GetConnectionProtocol()
        {
            var serverInfo = 
                NetworkConfiguration.GetInstance()
                    .GetServerInfo(ServerType.Game);
            var protocol = serverInfo.Protocol;

            return protocol;
        }
    }
}