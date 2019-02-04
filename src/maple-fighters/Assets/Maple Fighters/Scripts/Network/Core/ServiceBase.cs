using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using PhotonClientImplementation;
using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.Services
{
    public class ServiceBase : MonoBehaviour
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

        public async Task<ConnectionStatus> Connect(
            IYield yield,
            ServerConnectionInformation serverConnectionInformation)
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

            return 
                serverPeer == null
                       ? ConnectionStatus.Failed
                       : ConnectionStatus.Succeed;
        }

        public void SetNetworkTrafficState(NetworkTrafficState state)
        {
            if (IsConnected())
            {
                serverPeer.NetworkTrafficState = state;
            }
        }

        private void Disconnect()
        {
            if (IsConnected())
            {
                serverPeer.Disconnect();
                serverPeer = null;
            }
        }

        private bool IsConnected()
        {
            return serverPeer != null && serverPeer.IsConnected;
        }
    }
}