using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using PhotonClientImplementation;
using Scripts.ScriptableObjects;

namespace Scripts.Services
{
    internal class ServiceConnectionHandler : IServiceConnectionHandler
    {
        public ServerConnectionInformation ServerConnectionInformation { get; private set; }
        public IPeerDisconnectionNotifier PeerDisconnectionNotifier => serverPeer.PeerDisconnectionNotifier;

        private IServerPeer serverPeer;
        private readonly Action<IServerPeer> onConnected;

        public ServiceConnectionHandler(Action<IServerPeer> onConnected)
        {
            this.onConnected = onConnected;
        }

        public async Task<ConnectionStatus> Connect(IYield yield, ICoroutinesExecutor coroutinesExecutor, ServerConnectionInformation serverConnectionInformation)
        {
            var serverType = serverConnectionInformation.ServerType;

            if (IsConnected())
            {
                throw new ServerConnectionFailed($"A connection already exists with a {serverType} server.");
            }

            ServerConnectionInformation = serverConnectionInformation;

            var ip = ServerConnectionInformation.PeerConnectionInformation.Ip;
            var port = ServerConnectionInformation.PeerConnectionInformation.Port;
            LogUtils.Log($"Connecting to a {serverType} server. IP: {ip} Port: {port}");

            var serverConnector = new PhotonServerConnector(() => coroutinesExecutor);
            var networkConfiguration = NetworkConfiguration.GetInstance();
            var connectionDetails = new ConnectionDetails(networkConfiguration.ConnectionProtocol, networkConfiguration.DebugLevel);

            serverPeer = await serverConnector.ConnectAsync(yield, ServerConnectionInformation.PeerConnectionInformation, connectionDetails);
            if (serverPeer == null)
            {
                return ConnectionStatus.Failed;
            }

            onConnected?.Invoke(serverPeer);
            return ConnectionStatus.Succeed;
        }

        public void SetNetworkTrafficState(NetworkTrafficState state)
        {
            if (IsConnected())
            {
                serverPeer.NetworkTrafficState = state;
            }
        }

        public void Dispose()
        {
            if (IsConnected())
            {
                Disconnect();
            }
        }

        private void Disconnect()
        {
            serverPeer.Disconnect();
            serverPeer = null;
        }

        public bool IsConnected()
        {
            return serverPeer != null && serverPeer.IsConnected;
        }
    }
}