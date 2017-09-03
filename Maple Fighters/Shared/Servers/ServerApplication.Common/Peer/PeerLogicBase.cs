using System;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.Peer
{
    public abstract class PeerLogicBase<TOperationCode, TEventCode> : ClientPeerWrapper<IClientPeer>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestHandlerRegister<TOperationCode> OperationRequestHandlerRegister { get; private set; }
        protected IEventSender<TEventCode> EventSender { get; private set; }

        private bool logOperationRequests;
        private bool logOperationResponses;
        private bool logEvents;

        protected PeerLogicBase(IClientPeer peer, int peerId) 
            : base(peer, peerId)
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

        public override void SendEvent<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
        {
            var gameEvent = (TEventCode)Enum.ToObject(typeof(TEventCode), code);
            EventSender.Send(gameEvent, parameters, messageSendOptions);
        }
    }
}