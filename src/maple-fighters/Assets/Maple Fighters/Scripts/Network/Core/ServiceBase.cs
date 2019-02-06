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
        private IServerConnector serverConnector;
        private IServerPeer serverPeer;
        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            Disconnect();

            coroutinesExecutor?.Dispose();
        }

        public void Connect(ConnectionInformation connectionInformation)
        {
            if (coroutinesExecutor == null)
            {
                coroutinesExecutor = new ExternalCoroutinesExecutor();
            }

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

        public void SetNetworkTrafficState(NetworkTrafficState state)
        {
            if (IsConnected())
            {
                serverPeer.NetworkTrafficState = state;
            }
        }

        public void Disconnect()
        {
            if (serverPeer != null)
            {
                if (IsConnected())
                {
                    serverPeer.Disconnect();
                }

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

            if (serverPeer != null)
            {
                OnConnected();
            }

            return
                serverPeer != null
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