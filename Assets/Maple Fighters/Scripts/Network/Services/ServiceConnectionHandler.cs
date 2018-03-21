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
        public IServerPeer ServerPeer { get; private set; }

        public ServerConnectionInformation ServerConnectionInformation { get; private set; }
        public IPeerDisconnectionNotifier PeerDisconnectionNotifier => ServerPeer.PeerDisconnectionNotifier;

        private ServerType serverType;
        private readonly Action<IServerPeer> onConnected;

        public ServiceConnectionHandler(Action<IServerPeer> onConnected)
        {
            this.onConnected = onConnected;
        }

        public async Task<ConnectionStatus> Connect(IYield yield, ICoroutinesExecutor coroutinesExecutor, ServerConnectionInformation serverConnectionInformation)
        {
            serverType = serverConnectionInformation.ServerType;

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

            ServerPeer = await serverConnector.ConnectAsync(yield, ServerConnectionInformation.PeerConnectionInformation, connectionDetails);
            if (ServerPeer == null)
            {
                return ConnectionStatus.Failed;
            }

            onConnected?.Invoke(ServerPeer);
            return ConnectionStatus.Succeed;
        }

        public void SetNetworkTrafficState(NetworkTrafficState state)
        {
            if (IsConnected())
            {
                ServerPeer.NetworkTrafficState = state;
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
            ServerPeer.Disconnect();
            ServerPeer = null;
        }

        public bool IsConnected()
        {
            return ServerPeer != null && ServerPeer.IsConnected;
        }
    }
}