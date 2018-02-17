using System;
using System.Collections.Generic;
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
        private IOperationRequestSender<TOperationCode> operationRequestSender;
        private IEventHandlerRegister<TEventCode> eventHandlerRegister;
        private IOperationResponseSubscriptionProvider subscriptionProvider;

        private bool disposed;

        protected override void OnAwake()
        {
            base.OnAwake();

            StartConnectContinuously();
        }

        private void StartConnectContinuously()
        {
            var coroutinesExecutor = Components.GetComponent<ICoroutinesExecuter>().AssertNotNull();
            coroutinesExecutor.StartCoroutine(ConnectContinuously());
        }

        private IEnumerator<IYieldInstruction> ConnectContinuously()
        {
            const int WAIT_TIME = 30;

            outboundServerPeer = null;

            while (true)
            {
                if (IsConnected())
                {
                    yield break;
                }

                var coroutinesExecutor = Components.GetComponent<ICoroutinesExecuter>().AssertNotNull();
                coroutinesExecutor.StartTask(Connect);
                yield return new WaitForSeconds(WAIT_TIME);
            }
        }

        private async Task Connect(IYield yield)
        {
            var peerConnectionInformation = GetPeerConnectionInformation();

            try
            {
                LogUtils.Log($"An attempt to connect to a server - {peerConnectionInformation.Ip}:{peerConnectionInformation.Port}");

                var serverConnector = Components.GetComponent<IServerConnectorProvider>().AssertNotNull();
                outboundServerPeer = await serverConnector.GetServerConnector().Connect(yield, peerConnectionInformation);
            }
            catch (CouldNotConnectToPeerException exception)
            {
                if (exception.Message != string.Empty)
                {
                    LogUtils.Log(MessageBuilder.Trace(exception.Message));
                }

                LogUtils.Log($"Could not connect to a server - {peerConnectionInformation.Ip}:{peerConnectionInformation.Port}");
            }
            finally
            {
                if (IsConnected())
                {
                    OnConnected();
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            disposed = true;

            if (IsConnected())
            {
                outboundServerPeer.Disconnect();
                outboundServerPeer = null;
            }

            subscriptionProvider?.Dispose();
            eventHandlerRegister?.Dispose();
        }

        protected virtual void OnConnected()
        {
            SubscribeToDisconnectionNotifier();
            InitializePeerHandlers();

            var peerConnectionInformation = GetPeerConnectionInformation();
            LogUtils.Log($"A connection with {peerConnectionInformation.Ip}:{peerConnectionInformation.Port} has been established successfully.");

            outboundServerPeer.NetworkTrafficState = NetworkTrafficState.Flowing;
        }

        protected virtual void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            UnsubscribeFromDisconnectionNotifier();

            var peerConnectionInformation = GetPeerConnectionInformation();
            LogUtils.Log($"Disconnected from a server - {peerConnectionInformation.Ip}:{peerConnectionInformation.Port}");

            if (!disposed)
            {
                StartConnectContinuously();
            }
        }

        private void OnOperationRequestFailed(RawMessageResponseData data, short requestId)
        {
            var peerConnectionInformation = GetPeerConnectionInformation();
            LogUtils.Log($"Sending an operaiton has been failed. Operation Code: {data.Code} Server Details: " +
                         $"{peerConnectionInformation.Ip}:{peerConnectionInformation.Port}");
        }

        private void InitializePeerHandlers()
        {
            var logOperationsRequest = (bool)Config.Global.Log.OperationsRequest;
            var logOperationsResponse = (bool)Config.Global.Log.OperationsResponse;
            var logEvents = (bool)Config.Global.Log.Events;

            operationRequestSender = new OperationRequestSender<TOperationCode>(outboundServerPeer.OperationRequestSender, logOperationsRequest);
            subscriptionProvider = new OperationResponseSubscriptionProvider<TOperationCode>(outboundServerPeer.OperationResponseNotifier, OnOperationRequestFailed, logOperationsResponse);
            eventHandlerRegister = new EventHandlerRegister<TEventCode>(outboundServerPeer.EventNotifier, logEvents);
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

        public void SendOperation<TParams>(byte operationCode, TParams parameters)
            where TParams : struct, IParameters
        {
            if (!IsConnected())
            {
                LogUtils.Log($"Could not send {operationCode} operation because no connection to a server.");
                return;
            }

            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
            operationRequestSender.Send(code, parameters, MessageSendOptions.DefaultReliable());
        }

        public async Task<TResponseParams> SendYieldOperation<TRequestParams, TResponseParams>(IYield yield, byte operationCode, TRequestParams parameters)
            where TRequestParams : struct, IParameters
            where TResponseParams : struct, IParameters
        {
            if (!IsConnected())
            {
                LogUtils.Log($"Could not send {operationCode} operation because no connection to a server.");
                return default(TResponseParams);
            }

            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
            var requestId = operationRequestSender.Send(code, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await subscriptionProvider.ProvideSubscription<TResponseParams>(yield, requestId);
            return responseParameters;
        }

        protected void SetEventHandler<TParams>(TEventCode eventCode, Action<TParams> action)
            where TParams : struct, IParameters
        {
            var eventHandler = new EventHandler<TParams>((x) => action?.Invoke(x.Parameters));
            eventHandlerRegister.SetHandler(eventCode, eventHandler);
        }

        protected void RemoveEventHandler(TEventCode eventCode)
        {
            eventHandlerRegister.RemoveHandler(eventCode);
        }

        public bool IsConnected()
        {
            return outboundServerPeer != null && outboundServerPeer.IsConnected;
        }

        protected abstract PeerConnectionInformation GetPeerConnectionInformation();
    }
}