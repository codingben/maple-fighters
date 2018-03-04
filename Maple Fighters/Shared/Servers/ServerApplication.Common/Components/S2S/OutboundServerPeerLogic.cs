using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using JsonConfig;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public class OutboundServerPeerLogic<TOperationCode, TEventCode> : IOutboundServerPeerLogic
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private readonly IOutboundServerPeer outboundServerPeer;
        private readonly IOperationRequestSender<TOperationCode> operationRequestSender;
        private readonly IEventHandlerRegister<TEventCode> eventHandlerRegister;
        private readonly IOperationResponseSubscriptionProvider subscriptionProvider;

        public OutboundServerPeerLogic(IOutboundServerPeer outboundServerPeer)
        {
            this.outboundServerPeer = outboundServerPeer;

            var logOperationsRequest = (bool)Config.Global.Log.OperationsRequest;
            var logOperationsResponse = (bool)Config.Global.Log.OperationsResponse;
            var logEvents = (bool)Config.Global.Log.Events;

            operationRequestSender = new OperationRequestSender<TOperationCode>(outboundServerPeer.OperationRequestSender, logOperationsRequest);
            subscriptionProvider = new OperationResponseSubscriptionProvider<TOperationCode>(outboundServerPeer.OperationResponseNotifier, OnOperationRequestFailed, logOperationsResponse);
            eventHandlerRegister = new EventHandlerRegister<TEventCode>(outboundServerPeer.EventNotifier, logEvents);
        }

        public void Dispose()
        {
            eventHandlerRegister?.Dispose();
            subscriptionProvider?.Dispose();
        }

        public void SendOperation<TParams>(byte operationCode, TParams parameters)
            where TParams : struct, IParameters
        {
            if (!IsConnected())
            {
                // If there is no connection, an operation request is not will be sent.
                return;
            }

            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
            operationRequestSender.Send(code, parameters, MessageSendOptions.DefaultReliable());
        }

        public async Task<TResponseParams> SendOperation<TRequestParams, TResponseParams>(IYield yield, byte operationCode, TRequestParams parameters)
            where TRequestParams : struct, IParameters
            where TResponseParams : struct, IParameters
        {
            if (!IsConnected())
            {
                // If there is no connection, an operation request is not will be sent.
                return default(TResponseParams);
            }

            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
            var requestId = operationRequestSender.Send(code, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await subscriptionProvider.ProvideSubscription<TResponseParams>(yield, requestId);
            return responseParameters;
        }

        public void SetEventHandler<TParams>(byte eventCode, Action<TParams> action)
            where TParams : struct, IParameters
        {
            var code = (TEventCode)Enum.ToObject(typeof(TEventCode), eventCode);
            var eventHandler = new EventHandler<TParams>((x) => action?.Invoke(x.Parameters));
            eventHandlerRegister.SetHandler(code, eventHandler);
        }

        public void RemoveEventHandler(byte eventCode)
        {
            var code = (TEventCode)Enum.ToObject(typeof(TEventCode), eventCode);
            eventHandlerRegister.RemoveHandler(code);
        }

        private void OnOperationRequestFailed(RawMessageResponseData data, short requestId)
        {
            LogUtils.Log($"Sending an operaiton has been failed. Operation Code: {data.Code}");
        }

        private bool IsConnected()
        {
            return outboundServerPeer.IsConnected;
        }
    }
}