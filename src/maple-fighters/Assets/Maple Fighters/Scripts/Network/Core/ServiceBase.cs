using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using PhotonClientImplementation;
using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.Network.Core
{
    public class ServiceBase : MonoBehaviour, IServiceBase
    {
        private IServerConnector serverConnector;
        private IServerPeer serverPeer;

        private ExternalCoroutinesExecutor coroutinesExecutor;
        private NetworkConfiguration networkConfiguration;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            networkConfiguration = NetworkConfiguration.GetInstance();

            OnAwake();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.Dispose();

            OnDestroying();
        }

        public void Connect()
        {
            if (serverConnector == null)
            {
                if (GameConfiguration.GetInstance().Environment
                    == Environment.Production)
                {
                    serverConnector =
                        new PhotonServerConnector(() => coroutinesExecutor);
                }
                else
                {
                    serverConnector = new DummyPhotonServerConnector();
                }
            }

            coroutinesExecutor.StartTask(ConnectAsync);
        }

        public void SetNetworkTrafficState(NetworkTrafficState networkTrafficState)
        {
            if (IsConnected())
            {
                serverPeer.NetworkTrafficState = networkTrafficState;
            }
        }

        public void Disconnect()
        {
            if (IsConnected())
            {
                serverPeer.Disconnect();
                serverPeer = null;

                OnDisconnected();
            }
        }

        public bool IsConnected()
        {
            return serverPeer != null;
        }

        protected IServerPeer GetServerPeer()
        {
            return serverPeer;
        }

        private async Task<ConnectionStatus> ConnectAsync(IYield yield)
        {
            // TODO: Implement the IP:Port
            var peerConnectionInformation = new PeerConnectionInformation();
            var connectionDetails = 
                new ConnectionDetails(
                    networkConfiguration.ConnectionProtocol,
                    networkConfiguration.DebugLevel);

            serverPeer = 
                await serverConnector.ConnectAsync(
                    yield,
                    peerConnectionInformation,
                    connectionDetails);

            if (IsConnected())
            {
                OnConnected();
            }

            return
                IsConnected()
                    ? ConnectionStatus.Failed
                    : ConnectionStatus.Succeed;
        }

        protected virtual void OnAwake()
        {
            // Left blank intentionally
        }

        protected virtual void OnDestroying()
        {
            // Left blank intentionally
        }

        protected virtual void OnConnected()
        {
            // Left blank intentionally
        }

        protected virtual void OnDisconnected()
        {
            // Left blank intentionally
        }
    }
}