using System;
using CommunicationHelper;
using ServerCommunicationInterfaces;

namespace ServerCommon.Peer
{
    public abstract class OutboundS2SPeerBase<TO, TE> : IDisposable
        where TO : IComparable, IFormattable, IConvertible
        where TE : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestSender<TO> OperationRequestSender
        {
            get;
        }

        protected IEventHandlerRegister<TE> EventHandlerRegister
        {
            get;
        }

        protected IOperationResponseSubscriptionProvider SubscriptionProvider
        {
            get;
        }

        protected OutboundS2SPeerBase(IOutboundServerPeer peer, bool log = false)
        {
            OperationRequestSender = new OperationRequestSender<TO>(peer.OperationRequestSender, log);
            EventHandlerRegister = new EventHandlerRegister<TE>(peer.EventNotifier, log);
            SubscriptionProvider = new OperationResponseSubscriptionProvider<TO>(peer.OperationResponseNotifier, null, log);
        }

        public void Dispose()
        {
            EventHandlerRegister?.Dispose();
            SubscriptionProvider?.Dispose();
        }
    }
}