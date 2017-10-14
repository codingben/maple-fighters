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

namespace Scripts.Services
{
    public enum ConnectionStatus
    {
        Succeed,
        Failed
    }

    public abstract class ServiceBase<TOperationCode, TEventCode> : IDisposable
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private IServerPeer serverPeer;
        private ServersType serverType;
        private PeerConnectionInformation peerConnectionInformation;

        protected IEventHandlerRegister<TEventCode> EventHandlerRegister { get; private set; }
        protected IOperationRequestSender<TOperationCode> OperationRequestSender { get; private set; }
        protected IOperationResponseSubscriptionProvider SubscriptionProvider { get; private set; }

        protected readonly ExternalCoroutinesExecutor CoroutinesExecutor = new ExternalCoroutinesExecutor().ExecuteExternally();

        public void Dispose()
        {
            serverPeer?.Disconnect();

            SubscriptionProvider?.Dispose();
            EventHandlerRegister?.Dispose();
        }

        protected async Task<ConnectionStatus> Connect(IYield yield, ConnectionInformation connectionInformation)
        {
            var peerConnectionInformation = GetPeerConnectionInformation(connectionInformation);

            LogUtils.Log($"Connecting to a {serverType} server. IP: {peerConnectionInformation.Ip} Port: {peerConnectionInformation.Port}");

            var serverConnector = new PhotonServerConnector(() => CoroutinesExecutor);
            var networkConfiguration = NetworkConfiguration.GetInstance();

            serverPeer = await serverConnector.ConnectAsync(yield, peerConnectionInformation,
                new ConnectionDetails(networkConfiguration.ConnectionProtocol, networkConfiguration.DebugLevel));

            if (serverPeer == null)
            {
                return ConnectionStatus.Failed;
            }

            InitializePeerHandlers();
            OnConnected();
            return ConnectionStatus.Succeed;
        }

        protected abstract void OnConnected();

        protected abstract void OnDisconnected();

        private void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            serverPeer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;

            LogUtils.Log("A connection has been closed with " +
                            $"{serverType} - {peerConnectionInformation.Ip}:{peerConnectionInformation.Port}. Reason: {disconnectReason}");

            OnDisconnected();
        }

        private void InitializePeerHandlers()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();

            OperationRequestSender = new OperationRequestSender<TOperationCode>(serverPeer.OperationRequestSender, networkConfiguration.LogOperationsRequest);
            SubscriptionProvider = new OperationResponseSubscriptionProvider<TOperationCode>(serverPeer.OperationResponseNotifier, OnOperationRequestFailed, 
                networkConfiguration.LogOperationsResponse);
            EventHandlerRegister = new EventHandlerRegister<TEventCode>(serverPeer.EventNotifier, networkConfiguration.LogEvents);

            serverPeer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void OnOperationRequestFailed(RawMessageResponseData data, short requestId)
        {
            LogUtils.Log(MessageBuilder.Trace($"Sending an operaiton has been failed. Operation Code: {data.Code} - Server Type: {serverType}"));
        }

        private void OnApplicationQuit()
        {
            Dispose();
        }

        protected bool IsServerConnected()
        {
            return serverPeer != null && serverPeer.IsConnected;
        }

        private PeerConnectionInformation GetPeerConnectionInformation(ConnectionInformation connectionInformation)
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();

            serverType = connectionInformation.ServerType;

            switch (networkConfiguration.ConnectionProtocol)
            {
                case ConnectionProtocol.Udp:
                {
                    peerConnectionInformation = connectionInformation.UdpConnectionDetails;
                    break;
                }
                case ConnectionProtocol.Tcp:
                {
                    peerConnectionInformation = connectionInformation.TcpConnectionDetails;
                    break;
                }
                case ConnectionProtocol.WebSocket:
                case ConnectionProtocol.WebSocketSecure:
                {
                    peerConnectionInformation = connectionInformation.WebConnectionDetails;
                    break;
                }
            }

            return new PeerConnectionInformation(peerConnectionInformation.Ip, peerConnectionInformation.Port);
        }
    }
}