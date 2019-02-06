using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using PhotonClientImplementation;
using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.Network
{
    public class ServiceBase : MonoBehaviour, IServiceBase
    {
        private IServerConnector serverConnector;
        private IServerPeer serverPeer;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();

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

        public void Connect(ConnectionInformation connectionInformation)
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

            coroutinesExecutor.StartTask(
                (yield) => ConnectAsync(yield, connectionInformation));
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

        public IServerPeer GetServerPeer()
        {
            return serverPeer;
        }

        private async Task<ConnectionStatus> ConnectAsync(
            IYield yield,
            ConnectionInformation connectionInformation)
        {
            var connectionDetails = 
                new ConnectionDetails(
                    NetworkConfiguration.GetInstance().ConnectionProtocol,
                    NetworkConfiguration.GetInstance().DebugLevel);

            serverPeer = 
                await serverConnector.ConnectAsync(
                    yield,
                    connectionInformation.PeerConnectionInformation,
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