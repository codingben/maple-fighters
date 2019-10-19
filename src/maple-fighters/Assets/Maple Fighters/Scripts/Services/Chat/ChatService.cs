using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using Network.Scripts;
using Network.Utils;
using Scripts.Services.Authorizer;

namespace Scripts.Services.Chat
{
    public class ChatService : Singleton<ChatService>, IChatService
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

        public IChatApi ChatApi
        {
            get
            {
                if (chatApi == null)
                {
                    chatApi = new DummyChatApi(serverPeer);
                }

                return chatApi;
            }
        }

        private IAuthorizerApi authorizerApi;
        private IChatApi chatApi;
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
            ((IDisposable)chatApi)?.Dispose();
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