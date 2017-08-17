using System;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace Shared.Communication.Common.Peer
{
    public abstract class PeerLogic<TOperationCode, TEventCode> : ClientPeer<IClientPeer>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestHandlerRegister<TOperationCode> OperationRequestHandlerRegister { get; private set; }
        protected IEventSender<TEventCode> EventSender { get; private set; }

        private bool logOperationRequests;
        private bool logOperationResponses;
        private bool logEvents;

        protected PeerLogic(IClientPeer peer) 
            : base(peer)
        {
            ActivateLogs(operationRequets: true, operationResponses: true, events: true);
            InitializeCommunication();
        }

        protected void ActivateLogs(bool operationRequets, bool operationResponses, bool events)
        {
            logOperationRequests = operationRequets;
            logOperationResponses = operationResponses;
            logEvents = events;
        }

        private void InitializeCommunication()
        {
            var operationRequestNotifier = Peer.OperationRequestNotifier;
            var operationResponseSender = Peer.OperationResponseSender;
            var eventSender = Peer.EventSender;

            OperationRequestHandlerRegister = new OperationRequestsHandler<TOperationCode>(
                operationRequestNotifier,
                operationResponseSender,
                logOperationRequests,
                logOperationResponses
            );

            EventSender = new EventSender<TEventCode>(
                eventSender,
                logEvents
            );
        }
    }
}