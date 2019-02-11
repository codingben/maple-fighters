using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using CommunicationHelper;
using Scripts.ScriptableObjects;

namespace Scripts.Network.Core
{
    public class ApiBase<TOperationCode, TEventCode> : IApiBase
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestSender<TOperationCode> OperationRequestSender
        {
            get;
            private set;
        }

        protected IEventHandlerRegister<TEventCode> EventHandlerRegister
        {
            get;
            private set;
        }

        protected IOperationResponseSubscriptionProvider SubscriptionProvider
        {
            get;
            private set;
        }

        public void SetServerPeer(IServerPeer serverPeer)
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();

            OperationRequestSender =
                new OperationRequestSender<TOperationCode>(
                    serverPeer.OperationRequestSender,
                    networkConfiguration.LogOperationsRequest);

            SubscriptionProvider =
                new OperationResponseSubscriptionProvider<TOperationCode>(
                    serverPeer.OperationResponseNotifier,
                    OnOperationRequestFailed,
                    networkConfiguration.LogOperationsResponse);

            EventHandlerRegister =
                new EventHandlerRegister<TEventCode>(
                    serverPeer.EventNotifier,
                    networkConfiguration.LogEvents);

            OnSetServerPeer();
        }

        public void Dispose()
        {
            EventHandlerRegister?.Dispose();
            SubscriptionProvider?.Dispose();

            OnUnsetServerPeer();
        }

        private void OnOperationRequestFailed(
            RawMessageResponseData data,
            short requestId)
        {
            LogUtils.Log($"Sending an operation has been failed. Operation Code: {data.Code}");
        }

        protected virtual void OnSetServerPeer()
        {
            // Left blank intentionally
        }

        protected virtual void OnUnsetServerPeer()
        {
            // Left blank intentionally
        }
    }
}