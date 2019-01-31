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
        public IServiceConnectionNotifier ConnectionNotifier => serviceConnectionNotifier;

        public IServerPeer ServerPeer { get; private set; }

        private ServerConnectionInformation ConnectionInformation { get; set; }

        private readonly ServiceConnectionNotifier serviceConnectionNotifier;

        public ServiceConnectionHandler()
        {
            serviceConnectionNotifier = new ServiceConnectionNotifier();
        }

        public async Task<ConnectionStatus> Connect(
            IYield yield,
            ICoroutinesExecutor coroutinesExecutor,
            ServerConnectionInformation serverConnectionInformation)
        {
            var serverType = serverConnectionInformation.ServerType;

            if (IsConnected())
            {
                throw new ServerConnectionFailed(
                    $"A connection already exists with a {serverType} server.");
            }

            ConnectionInformation = serverConnectionInformation;

            var ip = ConnectionInformation.PeerConnectionInformation.Ip;
            var port = ConnectionInformation.PeerConnectionInformation.Port;

            LogUtils.Log(
                $"Connecting to a {serverType} server. IP: {ip} Port: {port}");

            var serverConnector =
                new PhotonServerConnector(() => coroutinesExecutor);
            var connectionDetails = 
                new ConnectionDetails(
                    NetworkConfiguration.GetInstance().ConnectionProtocol,
                    NetworkConfiguration.GetInstance().DebugLevel);

            ServerPeer = 
                await serverConnector.ConnectAsync(
                    yield,
                    ConnectionInformation.PeerConnectionInformation,
                    connectionDetails);

            if (ServerPeer == null)
            {
                return ConnectionStatus.Failed;
            }

            SubscribeToDisconnectionNotifier();
            
            LogUtils.Log(
                $"A {serverType} server has been connected: {ip}:{port}");

            serviceConnectionNotifier.Connection();

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

        private void SubscribeToDisconnectionNotifier()
        {
            ServerPeer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            ServerPeer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        private void Disconnect()
        {
            ServerPeer.Disconnect();
            ServerPeer = null;
        }

        private void OnDisconnected(DisconnectReason reason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            var ip = ConnectionInformation.PeerConnectionInformation.Ip;
            var port = ConnectionInformation.PeerConnectionInformation.Port;

            LogUtils.Log(
                $"The connection has been closed with {ip}:{port}. Reason: {reason}");

            serviceConnectionNotifier.Disconnection(reason, details);
        }

        public bool IsConnected()
        {
            return ServerPeer != null && ServerPeer.IsConnected;
        }
    }
}