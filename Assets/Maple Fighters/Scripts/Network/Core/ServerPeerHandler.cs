using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using Scripts.ScriptableObjects;

namespace Scripts.Services
{
    public class ServerPeerHandler<TOperationCode, TEventCode> : IServerPeerHandler
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private IServerPeer serverPeer;
        private IOperationRequestSender<TOperationCode> operationRequestSender;
        private IEventHandlerRegister<TEventCode> eventHandlerRegister;
        private IOperationResponseSubscriptionProvider subscriptionProvider;

        public void Initialize(IServerPeer peer)
        {
            serverPeer = peer;

            var networkConfiguration = NetworkConfiguration.GetInstance();
            operationRequestSender = new OperationRequestSender<TOperationCode>(serverPeer.OperationRequestSender, networkConfiguration.LogOperationsRequest);
            subscriptionProvider = new OperationResponseSubscriptionProvider<TOperationCode>(serverPeer.OperationResponseNotifier, OnOperationRequestFailed,
                networkConfiguration.LogOperationsResponse);
            eventHandlerRegister = new EventHandlerRegister<TEventCode>(serverPeer.EventNotifier, networkConfiguration.LogEvents);
        }

        public void Dispose()
        {
            eventHandlerRegister?.Dispose();
            subscriptionProvider?.Dispose();
        }

        public void SendOperation<TParams>(byte operationCode, TParams parameters, MessageSendOptions messageSendOptions)
            where TParams : struct, IParameters
        {
            if (!IsConnected())
            {
                return;
            }

            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
            operationRequestSender.Send(code, parameters, messageSendOptions);
        }

        public async Task<TResponseParams> SendOperation<TRequestParams, TResponseParams>(IYield yield, byte operationCode, TRequestParams parameters, MessageSendOptions messageSendOptions)
            where TRequestParams : struct, IParameters
            where TResponseParams : struct, IParameters
        {
            if (!IsConnected())
            {
                return default(TResponseParams);
            }

            var code = (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
            var requestId = operationRequestSender.Send(code, parameters, messageSendOptions);
            var responseParameters = await subscriptionProvider.ProvideSubscription<TResponseParams>(yield, requestId);
            return responseParameters;
        }

        public void SetEventHandler<TParams>(byte eventCode, UnityEvent<TParams> action)
            where TParams : struct, IParameters
        {
            var eventHandler = new EventHandler<TParams>((x) => action?.Invoke(x.Parameters));
            var code = (TEventCode)Enum.ToObject(typeof(TEventCode), eventCode);
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
            return serverPeer != null && serverPeer.IsConnected;
        }
    }
}