using System;
using ClientCommunicationInterfaces;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using Network.Scripts;
using PhotonClientImplementation;
using ScriptableObjects.Configurations;

namespace Scripts.Services.Authenticator
{
    public class AuthenticatorService : NetworkService
    {
        public IAuthenticatorApi AuthenticatorApi { get; set; }

        private IServerPeer authenticatorPeer;

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
            authenticatorPeer?.Disconnect();
        }

        private void OnDestroy()
        {
            ((IDisposable)AuthenticatorApi)?.Dispose();

            coroutinesExecutor?.Dispose();
        }

        protected override void OnConnected(IServerPeer serverPeer)
        {
            authenticatorPeer = serverPeer;

            var isDummy = NetworkConfiguration.GetInstance().IsDummy();
            if (isDummy)
            {
                AuthenticatorApi = new DummyAuthenticatorApi(serverPeer);
            }
            else
            {
                AuthenticatorApi = new AuthenticatorApi(serverPeer);
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
            var serverInfo = NetworkConfiguration.GetInstance().GetServerInfo(ServerType.Authenticator);
            var ip = serverInfo.IpAddress;
            var port = serverInfo.Port;

            return new PeerConnectionInformation(ip, port);
        }

        protected override ConnectionProtocol GetConnectionProtocol()
        {
            var serverInfo = NetworkConfiguration.GetInstance().GetServerInfo(ServerType.Authenticator);
            var protocol = serverInfo.Protocol;

            return protocol;
        }
    }
}