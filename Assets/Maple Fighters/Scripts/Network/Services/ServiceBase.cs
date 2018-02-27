using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using ExitGames.Client.Photon;
using PhotonClientImplementation;
using Scripts.ScriptableObjects;
using Scripts.Utils;

namespace Scripts.Services
{
    public abstract class ServiceBase<TOperationCode, TEventCode> : IServiceBase
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private IServerPeer serverPeer;
        private ServersType serverType;
        private PeerConnectionInformation peerConnectionInformation;

        private IEventHandlerRegister<TEventCode> EventHandlerRegister { get; set; }
        protected IOperationRequestSender<TOperationCode> OperationRequestSender { get; private set; }
        protected IOperationResponseSubscriptionProvider SubscriptionProvider { get; private set; }

        NetworkTrafficState? IServiceBase.NetworkTrafficState() => serverPeer?.NetworkTrafficState;

        public void SetNetworkTrafficState(NetworkTrafficState state)
        {
            if (IsConnected())
            {
                serverPeer.NetworkTrafficState = state;
            }
        }

        public async Task<ConnectionStatus> Connect(IYield yield, ICoroutinesExecutor coroutinesExecutor, ConnectionInformation connectionInformation)
        {
            var peerConnectionInformation = GetPeerConnectionInformation(connectionInformation);

            LogUtils.Log($"Connecting to a {serverType} server. IP: {peerConnectionInformation.Ip} Port: {peerConnectionInformation.Port}");

            var serverConnector = new PhotonServerConnector(() => coroutinesExecutor);
            var networkConfiguration = NetworkConfiguration.GetInstance();

            serverPeer = await serverConnector.ConnectAsync(yield, peerConnectionInformation,
                new ConnectionDetails(networkConfiguration.ConnectionProtocol, networkConfiguration.DebugLevel));

            if (serverPeer == null)
            {
                return ConnectionStatus.Failed;
            }

            InitializePeerHandlers();
            SubscribeToDisconnectionNotifier();
            OnConnected();
            return ConnectionStatus.Succeed;
        }

        public void Dispose()
        {
            if (!IsConnected() || serverPeer == null)
            {
                return;
            }

            serverPeer.Disconnect();

            SubscriptionProvider.Dispose();
            EventHandlerRegister.Dispose();
        }

        public bool IsConnected()
        {
            return serverPeer != null && serverPeer.IsConnected;
        }

        protected abstract void OnConnected();

        protected abstract void OnDisconnected();

        private void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            UnsubscribeFromDisconnectionNotifier();

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
        }

        public void SendOperation<TParams>(byte operationCode, TParams parameters, MessageSendOptions messageSendOptions)
            where TParams : struct, IParameters
        {
            if (!IsConnected())
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not send {operationCode} operation because no connection to a server."));
                return;
            }

            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
            OperationRequestSender.Send(code, parameters, messageSendOptions);
        }

        public async Task<TResponseParams> SendOperation<TRequestParams, TResponseParams>(IYield yield, byte operationCode, TRequestParams parameters, MessageSendOptions messageSendOptions)
            where TRequestParams : struct, IParameters
            where TResponseParams : struct, IParameters
        {
            if (!IsConnected())
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not send {operationCode} operation because no connection to a server."));
                return default(TResponseParams);
            }

            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
            var requestId = OperationRequestSender.Send(code, parameters, messageSendOptions);
            var responseParameters = await SubscriptionProvider.ProvideSubscription<TResponseParams>(yield, requestId);
            return responseParameters;
        }

        protected void SetEventHandler<TParams>(TEventCode eventCode, UnityEvent<TParams> action)
            where TParams : struct, IParameters
        {
            var eventHandler = new EventHandler<TParams>((x) => action?.Invoke(x.Parameters));
            EventHandlerRegister.SetHandler(eventCode, eventHandler);
        }

        protected void RemoveEventHandler(TEventCode eventCode)
        {
            EventHandlerRegister.RemoveHandler(eventCode);
        }

        private void OnOperationRequestFailed(RawMessageResponseData data, short requestId)
        {
            LogUtils.Log(MessageBuilder.Trace($"Sending an operaiton has been failed. Operation Code: {data.Code} - Server Type: {serverType}"));
        }

        private void SubscribeToDisconnectionNotifier()
        {
            serverPeer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            serverPeer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
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