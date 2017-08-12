using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using ExitGames.Client.Photon;
using PhotonClientImplementation;
using Scripts.Coroutines;
using Scripts.ScriptableObjects;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Services
{
    public abstract class ServiceBase<TOperationCode, TEventCode> : MonoBehaviour, IDisposable
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private const string NETWORK_CONFIGURATION_PATH = "Configurations/Network Configuration";

        [SerializeField] private ConnectionInformation connectionsInformation;

        private NetworkConfiguration networkConfiguration;

        private ServersType currentServerType;
        private PeerConnectionInformation currentConnectionInformation;

        private IServerPeer serverPeer;

        protected IEventHandlerRegister<TEventCode> EventHandlerRegister { get; private set; }
        protected IOperationRequestSender<TOperationCode> OperationRequestSender { get; private set; }
        protected IOperationResponseSubscriptionProvider SubscriptionProvider { get; private set; }

        protected ICoroutinesExecuter CoroutinesExecuter;

        private void Awake()
        {
            LogUtils.Logger = new Logger();

            CoroutinesExecuter = new CoroutinesExecuterWrapper();

            networkConfiguration = Resources.Load<NetworkConfiguration>(NETWORK_CONFIGURATION_PATH);

            Initiate();
        }

        public async Task<IServerPeer> ConnectAsync(Yield yield, PeerConnectionInformation connectionInformation)
        {
            var serverConnector = new PhotonServerConnector(() => CoroutinesExecuter);

            serverPeer = await serverConnector.ConnectAsync(yield, connectionInformation,
                new ConnectionDetails(networkConfiguration.ConnectionProtocol, networkConfiguration.DebugLevel));

            if (serverPeer == null)
            {
                return null;
            }

            InitializePeerHandlers();
            OnConnected();

            return serverPeer;
        }

        public void Dispose()
        {
            serverPeer?.Disconnect();

            SubscriptionProvider?.Dispose();
            EventHandlerRegister?.Dispose();
        }

        protected void Connect()
        {
            InitializePeer();

            Debug.Log($"Connecting to a {currentServerType} server - " + $"{currentConnectionInformation.Ip}:{currentConnectionInformation.Port}");

            CoroutinesExecuter.StartTask(y => ConnectAsync(y, currentConnectionInformation));
        }

        protected abstract void Initiate();

        protected abstract void OnConnected();

        protected abstract void OnDisconnected();

        protected bool IsConnected()
        {
            return serverPeer.IsConnected;
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            Debug.Log("ServiceBase::OnDisconnected() -> The connection has been closed with " +
                      $"{currentServerType} - {currentConnectionInformation.Ip}:{currentConnectionInformation.Port}. Reason: {disconnectReason}");

            serverPeer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;

            OnDisconnected();
        }

        private void InitializePeer()
        {
            switch (networkConfiguration.ConnectionProtocol)
            {
                case ConnectionProtocol.Udp:
                    currentServerType = connectionsInformation.ServerType;
                    currentConnectionInformation = connectionsInformation.UdpConnectionDetails;
                    break;
                case ConnectionProtocol.Tcp:
                    currentServerType = connectionsInformation.ServerType;
                    currentConnectionInformation = connectionsInformation.TcpConnectionDetails;
                    break;
                case ConnectionProtocol.WebSocket:
                case ConnectionProtocol.WebSocketSecure:
                    currentServerType = connectionsInformation.ServerType;
                    currentConnectionInformation = connectionsInformation.WebConnectionDetails;
                    break;
            }

            if (networkConfiguration.ConnectionProtocol == ConnectionProtocol.WebSocketSecure)
            {
                Debug.LogError($"Connection type {networkConfiguration} is not supported yet.");
                networkConfiguration.ConnectionProtocol = ConnectionProtocol.WebSocket;
            }
        }

        private void InitializePeerHandlers()
        {
            SubscriptionProvider = new OperationResponseSubscriptionProvider<TOperationCode>(serverPeer.OperationResponseNotifier,
                (data, s) => Debug.LogError($"Sending an operaiton has been failed. Operation Code: {data.Code} - Server Type: {currentServerType}"));
            EventHandlerRegister = new EventHandlerRegister<TEventCode>(serverPeer.EventNotifier);
            OperationRequestSender = new OperationRequestSender<TOperationCode>(serverPeer.OperationRequestSender);

            serverPeer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void OnApplicationQuit()
        {
            Dispose();
        }
    }
}