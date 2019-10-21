using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using CommunicationHelper;

namespace Network.Scripts
{
    public class NetworkApi<TOperationCode, TEventCode> : IDisposable
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestSender<TOperationCode> OperationRequestSender
        {
            get;
        }

        protected IEventHandlerRegister<TEventCode> EventHandlerRegister
        {
            get;
        }

        protected IOperationResponseSubscriptionProvider SubscriptionProvider
        {
            get;
        }

        protected NetworkApi(IServerPeer serverPeer, bool log = false)
        {
            OperationRequestSender =
                new OperationRequestSender<TOperationCode>(
                    serverPeer.OperationRequestSender,
                    log);
            SubscriptionProvider =
                new OperationResponseSubscriptionProvider<TOperationCode>(
                    serverPeer.OperationResponseNotifier,
                    OnOperationFailed,
                    log);
            EventHandlerRegister =
                new EventHandlerRegister<TEventCode>(
                    serverPeer.EventNotifier,
                    log);
        }

        public virtual void Dispose()
        {
            EventHandlerRegister?.Dispose();
            SubscriptionProvider?.Dispose();
        }

        private void OnOperationFailed(RawMessageResponseData rawMessage, short id)
        {
            LogUtils.Log($"Failed to send operation. Code: {rawMessage.Code}");
        }
    }
}