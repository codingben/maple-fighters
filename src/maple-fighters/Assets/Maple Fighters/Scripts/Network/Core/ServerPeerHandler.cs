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

        public void Initialize(IServerPeer serverPeer)
        {
            this.serverPeer = serverPeer;

            var networkConfiguration = NetworkConfiguration.GetInstance();
            operationRequestSender = 
                new OperationRequestSender<TOperationCode>(
                    serverPeer.OperationRequestSender,
                    networkConfiguration.LogOperationsRequest);
            subscriptionProvider =
                new OperationResponseSubscriptionProvider<TOperationCode>(
                    serverPeer.OperationResponseNotifier,
                    OnOperationRequestFailed,
                    networkConfiguration.LogOperationsResponse);
            eventHandlerRegister = 
                new EventHandlerRegister<TEventCode>(
                    serverPeer.EventNotifier,
                    networkConfiguration.LogEvents);
        }

        public void Dispose()
        {
            eventHandlerRegister?.Dispose();
            subscriptionProvider?.Dispose();
        }

        public void SendOperation<TParameters>(
            byte operationCode,
            TParameters parameters,
            MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            if (IsConnected())
            {
                var code = 
                    (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
                operationRequestSender.Send(
                    code,
                    parameters,
                    messageSendOptions);
            }
        }

        public async Task<TResponseParameters>
            SendOperation<TRequestParameters, TResponseParameters>(
                IYield yield,
                byte operationCode,
                TRequestParameters parameters,
                MessageSendOptions messageSendOptions)
            where TRequestParameters : struct, IParameters
            where TResponseParameters : struct, IParameters
        {
            if (IsConnected())
            {
                var code =
                    (TOperationCode)Enum.ToObject(typeof(TOperationCode), operationCode);
                var requestId = operationRequestSender.Send(
                    code,
                    parameters,
                    messageSendOptions);

                return 
                    await subscriptionProvider
                           .ProvideSubscription<TResponseParameters>(yield, requestId);
            }

            return default(TResponseParameters);
        }

        public void SetEventHandler<TParameters>(
            byte eventCode,
            UnityEvent<TParameters> action)
            where TParameters : struct, IParameters
        {
            var code =
                (TEventCode)Enum.ToObject(typeof(TEventCode), eventCode);
            var eventHandler =
                new EventHandler<TParameters>(
                    (x) => action?.Invoke(x.Parameters));

            eventHandlerRegister.SetHandler(code, eventHandler);
        }

        public void RemoveEventHandler(byte eventCode)
        {
            var code =
                (TEventCode)Enum.ToObject(typeof(TEventCode), eventCode);
            eventHandlerRegister.RemoveHandler(code);
        }

        private void OnOperationRequestFailed(
            RawMessageResponseData data,
            short requestId)
        {
            LogUtils.Log(
                $"Sending an operaiton has been failed. Operation Code: {data.Code}");
        }

        private bool IsConnected()
        {
            return serverPeer != null && serverPeer.IsConnected;
        }
    }
}