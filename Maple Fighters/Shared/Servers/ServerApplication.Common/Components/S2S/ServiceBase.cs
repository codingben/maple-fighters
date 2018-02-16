using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using ComponentModel.Common;
using JsonConfig;
using ServerApplication.Common.Components.Coroutines;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public abstract class ServiceBase<TOperationCode, TEventCode> : Component, IServiceBase
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private IOutboundServerPeer outboundServerPeer;
        private IOperationRequestSender<TOperationCode> OperationRequestSender { get; set; }
        private IEventHandlerRegister<TEventCode> EventHandlerRegister { get; set; }
        private IOperationResponseSubscriptionProvider SubscriptionProvider { get; set; }

        protected override void OnAwake()
        {
            base.OnAwake();

            var coroutinesExecutor = Components.GetComponent<ICoroutinesExecuter>().AssertNotNull();
            coroutinesExecutor.StartTask(Connect);
        }

        private async Task Connect(IYield yield)
        {
            try
            {
                var serverConnector = Components.GetComponent<IServerConnectorProvider>().AssertNotNull();
                outboundServerPeer = await serverConnector.GetServerConnector().Connect(yield, GetPeerConnectionInformation());
            }
            catch (CouldNotConnectToPeerException exception)
            {
                if (exception.Message != string.Empty)
                {
                    LogUtils.Log(MessageBuilder.Trace(exception.Message));
                }
            }
            finally
            {
                if (outboundServerPeer != null)
                {
                    OnConnected();
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            outboundServerPeer?.Disconnect();

            SubscriptionProvider?.Dispose();
            EventHandlerRegister?.Dispose();
        }

        protected virtual void OnConnected()
        {
            InitializePeerHandlers();
            SubscribeToDisconnectionNotifier();

            var ip = GetPeerConnectionInformation().Ip;
            var port = GetPeerConnectionInformation().Port;
            LogUtils.Log(MessageBuilder.Trace($"A connection with {ip}:{port} has been established successfully."));
        }

        protected virtual void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            UnsubscribeFromDisconnectionNotifier();
        }

        private void OnOperationRequestFailed(RawMessageResponseData data, short requestId)
        {
            var ip = GetPeerConnectionInformation().Ip;
            var port = GetPeerConnectionInformation().Port;
            LogUtils.Log(MessageBuilder.Trace($"Sending an operaiton has been failed. Operation Code: {data.Code} Server Details: {ip}:{port}"));
        }

        private void InitializePeerHandlers()
        {
            var logOperationsRequest = (bool)Config.Global.Log.OperationsRequest;
            var logOperationsResponse = (bool)Config.Global.Log.OperationsResponse;
            var logEvents = (bool)Config.Global.Log.Events;

            OperationRequestSender = new OperationRequestSender<TOperationCode>(outboundServerPeer.OperationRequestSender, logOperationsRequest);
            SubscriptionProvider = new OperationResponseSubscriptionProvider<TOperationCode>(outboundServerPeer.OperationResponseNotifier, OnOperationRequestFailed, logOperationsResponse);
            EventHandlerRegister = new EventHandlerRegister<TEventCode>(outboundServerPeer.EventNotifier, logEvents);
        }

        private void SubscribeToDisconnectionNotifier()
        {
            outboundServerPeer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            if (outboundServerPeer?.PeerDisconnectionNotifier != null)
            {
                outboundServerPeer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
            }
        }

        protected void SendOperation<TParams>(TOperationCode operationCode, TParams parameters)
            where TParams : struct, IParameters
        {
            if (!IsConnected())
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not send {operationCode} operation because no connection to a server."));
                return;
            }

            OperationRequestSender.Send(operationCode, parameters, MessageSendOptions.DefaultReliable());
        }

        protected void SetEventHandler<TParams>(TEventCode eventCode, Action<TParams> action)
            where TParams : struct, IParameters
        {
            var eventHandler = new EventHandler<TParams>((x) => action?.Invoke(x.Parameters));
            EventHandlerRegister.SetHandler(eventCode, eventHandler);
        }

        protected void RemoveEventHandler(TEventCode eventCode)
        {
            EventHandlerRegister.RemoveHandler(eventCode);
        }

        public bool IsConnected()
        {
            return outboundServerPeer != null && outboundServerPeer.IsConnected;
        }

        public abstract PeerConnectionInformation GetPeerConnectionInformation();
    }
}