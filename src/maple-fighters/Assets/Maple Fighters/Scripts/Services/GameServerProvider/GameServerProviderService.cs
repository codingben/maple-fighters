using System;
using ClientCommunicationInterfaces;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using Network.Scripts;
using PhotonClientImplementation;
using ScriptableObjects.Configurations;

namespace Scripts.Services.GameServerProvider
{
    public class GameServerProviderService : NetworkService
    {
        public IGameServerProviderApi GameServerProviderApi { get; private set; }

        public bool IsConnected => gameServerProviderPeer != null && gameServerProviderPeer.IsConnected;

        private IServerPeer gameServerProviderPeer;
        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDisable()
        {
            gameServerProviderPeer?.Disconnect();
        }

        private void OnDestroy()
        {
            ((IDisposable)GameServerProviderApi)?.Dispose();

            coroutinesExecutor?.Dispose();
        }

        protected override void OnConnected(IServerPeer serverPeer)
        {
            gameServerProviderPeer = serverPeer;

            var isDummy = NetworkConfiguration.GetInstance().IsDummy();
            if (isDummy)
            {
                GameServerProviderApi = new DummyGameServerProviderApi(serverPeer);
            }
            else
            {
                GameServerProviderApi = new GameServerProviderApi(serverPeer);
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
            var serverInfo = NetworkConfiguration.GetInstance().GetServerInfo(ServerType.GameServerProvider);
            var ip = serverInfo.IpAddress;
            var port = serverInfo.Port;

            return new PeerConnectionInformation(ip, port);
        }

        protected override ConnectionProtocol GetConnectionProtocol()
        {
            var serverInfo = NetworkConfiguration.GetInstance().GetServerInfo(ServerType.GameServerProvider);
            var protocol = serverInfo.Protocol;

            return protocol;
        }
    }
}