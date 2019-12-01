using System;
using ClientCommunicationInterfaces;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using Network.Scripts;
using PhotonClientImplementation;
using ScriptableObjects.Configurations;

namespace Scripts.Services.Chat
{
    public class ChatService : NetworkService
    {
        public IChatApi ChatApi { get; private set; }

        private IServerPeer chatPeer;
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

        private void OnDisable()
        {
            chatPeer?.Disconnect();
        }

        private void OnDestroy()
        {
            ((IDisposable)ChatApi)?.Dispose();

            coroutinesExecutor?.Dispose();
        }

        protected override void OnConnected(IServerPeer serverPeer)
        {
            chatPeer = serverPeer;

            var isDummy = NetworkConfiguration.GetInstance().IsDummy();
            if (isDummy)
            {
                ChatApi = new DummyChatApi(serverPeer);
            }
            else
            {
                ChatApi = new ChatApi(serverPeer);
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
            var serverInfo = NetworkConfiguration.GetInstance().GetServerInfo(ServerType.Chat);
            var ip = serverInfo.IpAddress;
            var port = serverInfo.Port;

            return new PeerConnectionInformation(ip, port);
        }

        protected override ConnectionProtocol GetConnectionProtocol()
        {
            var serverInfo = NetworkConfiguration.GetInstance().GetServerInfo(ServerType.Chat);
            var protocol = serverInfo.Protocol;

            return protocol;
        }
    }
}