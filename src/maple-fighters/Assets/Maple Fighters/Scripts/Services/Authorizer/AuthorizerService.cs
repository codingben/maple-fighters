using System;
using ClientCommunicationInterfaces;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using Network.Scripts;
using PhotonClientImplementation;
using ScriptableObjects.Configurations;

namespace Scripts.Services.Authorizer
{
    public class AuthorizerService : NetworkService
    {
        public IAuthorizerApi AuthorizerApi { get; private set; }

        private IServerPeer authorizerPeer;
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
            authorizerPeer?.Disconnect();
        }

        private void OnDestroy()
        {
            ((IDisposable)AuthorizerApi)?.Dispose();

            coroutinesExecutor?.Dispose();
        }

        protected override void OnConnected(IServerPeer serverPeer)
        {
            authorizerPeer = serverPeer;

            var isDummy = NetworkConfiguration.GetInstance().IsDummy();
            if (isDummy)
            {
                AuthorizerApi = new DummyAuthorizerApi(serverPeer);
            }
            else
            {
                AuthorizerApi = new AuthorizerApi(serverPeer);
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
            var serverInfo = 
                NetworkConfiguration.GetInstance()
                    .GetServerInfo(ServerType.Authorizer);
            var ip = serverInfo.IpAddress;
            var port = serverInfo.Port;

            return new PeerConnectionInformation(ip, port);
        }

        protected override ConnectionProtocol GetConnectionProtocol()
        {
            var serverInfo = 
                NetworkConfiguration.GetInstance()
                    .GetServerInfo(ServerType.Authorizer);
            var protocol = serverInfo.Protocol;

            return protocol;
        }
    }
}