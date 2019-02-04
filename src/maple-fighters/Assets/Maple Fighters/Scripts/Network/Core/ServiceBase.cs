using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using PhotonClientImplementation;
using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.Services
{
    public class ServiceBase : MonoBehaviour, IServiceBase
    {
        private IServerPeer serverPeer;
        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.Dispose();

            Disconnect();
        }

        public void Connect(ConnectionInformation connectionInformation)
        {
            coroutinesExecutor.StartTask(
                (yield) => ConnectAsync(yield, connectionInformation));
        }

        public void SetNetworkTrafficState(NetworkTrafficState state)
        {
            if (IsConnected())
            {
                serverPeer.NetworkTrafficState = state;
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
            return serverPeer != null && serverPeer.IsConnected;
        }

        public IServerPeer GetServerPeer()
        {
            return serverPeer;
        }

        private async Task<ConnectionStatus> ConnectAsync(
            IYield yield,
            ConnectionInformation serverConnectionInformation)
        {
            var serverConnector =
                new PhotonServerConnector(() => coroutinesExecutor);
            var connectionDetails =
                new ConnectionDetails(
                    NetworkConfiguration.GetInstance().ConnectionProtocol,
                    NetworkConfiguration.GetInstance().DebugLevel);

            serverPeer = 
                await serverConnector.ConnectAsync(
                    yield,
                    serverConnectionInformation.PeerConnectionInformation,
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