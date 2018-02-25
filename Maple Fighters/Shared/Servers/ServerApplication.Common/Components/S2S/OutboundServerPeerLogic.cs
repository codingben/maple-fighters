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
    public class OutboundServerPeerLogic<TOperationCode, TEventCode> : IDisposable
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private readonly IOperationRequestSender<TOperationCode> operationRequestSender;
        private readonly IEventHandlerRegister<TEventCode> eventHandlerRegister;
        private readonly IOperationResponseSubscriptionProvider subscriptionProvider;

        public OutboundServerPeerLogic(IOutboundServerPeer outboundServerPeer)
        {
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
            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
            operationRequestSender.Send(code, parameters, MessageSendOptions.DefaultReliable());
        }

        public async Task<TResponseParams> SendOperation<TRequestParams, TResponseParams>(IYield yield, byte operationCode, TRequestParams parameters)
            where TRequestParams : struct, IParameters
            where TResponseParams : struct, IParameters
        {
            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
            var requestId = operationRequestSender.Send(code, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await subscriptionProvider.ProvideSubscription<TResponseParams>(yield, requestId);
            return responseParameters;
        }

        public void SetEventHandler<TParams>(TEventCode eventCode, Action<TParams> action)
            where TParams : struct, IParameters
        {
            var eventHandler = new EventHandler<TParams>((x) => action?.Invoke(x.Parameters));
            eventHandlerRegister.SetHandler(eventCode, eventHandler);
        }

        public void RemoveEventHandler(TEventCode eventCode)
        {
            eventHandlerRegister.RemoveHandler(eventCode);
        }

        private void OnOperationRequestFailed(RawMessageResponseData data, short requestId)
        {
            LogUtils.Log($"Sending an operaiton has been failed. Operation Code: {data.Code}");
        }
    }
}