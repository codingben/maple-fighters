using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using Network.Scripts;
using Network.Utils;

namespace Scripts.Services.Authenticator
{
    public class AuthenticatorService : Singleton<AuthenticatorService>
    {
        public IAuthenticatorApi AuthenticatorApi
        {
            get
            {
                if (authenticatorApi == null)
                {
                    authenticatorApi = new DummyAuthenticatorApi(serverPeer);
                }

                return authenticatorApi;
            }
        }

        private IAuthenticatorApi authenticatorApi;
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
            ((IDisposable)authenticatorApi)?.Dispose();
            coroutinesExecutor?.Dispose();
        }

        private async Task ConnectAsync(IYield yield)
        {
            var serverConnector = new DummyServerConnector();
            var connectionInfo = new PeerConnectionInformation();
            var connectionProtocol = ConnectionProtocol.Tcp;

            serverPeer = await serverConnector.Connect(yield, connectionInfo, connectionProtocol);
        }
    }
}