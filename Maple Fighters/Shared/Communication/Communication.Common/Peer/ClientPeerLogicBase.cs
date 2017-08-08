using System;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace Shared.Communication.Common.Peer
{
    public abstract class PeerLogicBase<TOperationCode, TEventCode> : ClientPeer<IClientPeer>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestHandlerRegister<TOperationCode> OperationRequestHandlerRegister { get; private set; }
        protected IEventSender<TEventCode> EventSender { get; private set; }

        protected bool LogOperationRequests;
        protected bool LogOperationResponses;
        protected bool LogEvents;

        protected PeerLogicBase(IClientPeer peer) 
            : base(peer)
        {
            InitializeCommunication();
        }

        private void InitializeCommunication()
        {
            var operationRequestNotifier = Peer.OperationRequestNotifier;
            var operationResponseSender = Peer.OperationResponseSender;
            var eventSender = Peer.EventSender;

            OperationRequestHandlerRegister = new OperationRequestsHandler<TOperationCode>(
                operationRequestNotifier,
                operationResponseSender,
                LogOperationRequests,
                LogOperationResponses
            );

            EventSender = new EventSender<TEventCode>(
                eventSender,
                LogEvents
            );
        }

        protected void ActivateLogs(bool operationRequets, bool operationResponses, bool events)
        {
            LogOperationRequests = operationRequets;
            LogOperationResponses = operationResponses;
            LogEvents = events;
        }
    }
}