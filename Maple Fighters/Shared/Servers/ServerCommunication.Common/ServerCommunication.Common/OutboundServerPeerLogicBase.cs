using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using JsonConfig;
using ServerCommunicationInterfaces;

namespace ServerCommunication.Common
{
    public class OutboundServerPeerLogicBase<TOperationCode, TEventCode> : IOutboundServerPeerLogic
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private readonly IOutboundServerPeer outboundServerPeer;
        private IOperationRequestSender<TOperationCode> operationRequestSender;
        private IEventHandlerRegister<TEventCode> eventHandlerRegister;
        private IOperationResponseSubscriptionProvider subscriptionProvider;

        public OutboundServerPeerLogicBase(IOutboundServerPeer outboundServerPeer)
        {
            this.outboundServerPeer = outboundServerPeer;
        }

        public void Initialize()
        {
            LogUtils.Assert(Config.Global.Log, MessageBuilder.Trace("Could not find a log configuration."));

            var logOperationsRequest = (bool)Config.Global.Log.OperationsRequest;
            var logOperationsResponse = (bool)Config.Global.Log.OperationsResponse;
            var logEvents = (bool)Config.Global.Log.Events;

            operationRequestSender = new OperationRequestSender<TOperationCode>(outboundServerPeer.OperationRequestSender, logOperationsRequest);
            subscriptionProvider = new OperationResponseSubscriptionProvider<TOperationCode>(outboundServerPeer.OperationResponseNotifier, OnOperationRequestFailed, logOperationsResponse);
            eventHandlerRegister = new EventHandlerRegister<TEventCode>(outboundServerPeer.EventNotifier, logEvents);

            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
            // Left blank intentionally
        }

        public void Dispose()
        {
            eventHandlerRegister?.Dispose();
            subscriptionProvider?.Dispose();

            OnDispose();
        }

        protected virtual void OnDispose()
        {
            // Left blank intentionally
        }

        public void SendOperation<TParams>(byte operationCode, TParams parameters)
            where TParams : struct, IParameters
        {
            if (!IsConnected())
            {
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
                return default(TResponseParams);
            }

            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);

            try
            {
                var requestId = operationRequestSender.Send(code, parameters, MessageSendOptions.DefaultReliable());
                var responseParameters = await subscriptionProvider.ProvideSubscription<TResponseParams>(yield, requestId);
                return responseParameters;
            }
            catch (OperationNotHandledException)
            {
                LogUtils.Log($"Operation {code} not handled; sent a default operation response.");
                return default(TResponseParams);
            }
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
            return outboundServerPeer != null && outboundServerPeer.IsConnected;
        }
    }
}