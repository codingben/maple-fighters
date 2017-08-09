using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
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
            ActivateLogs(operationRequets: true, operationResponses: true, events: true);
            InitializeCommunication();
        }

        private void InitializeCommunication()
        {
            LogUtils.Log($"InitializeCommunication() -> State: {Peer.NetworkTrafficState} Connected: {Peer.IsConnected}");

            Peer.PeerDisconnectionNotifier.Disconnected += OnPeerDisconnected;

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

            Initialize();
        }

        protected void ActivateLogs(bool operationRequets, bool operationResponses, bool events)
        {
            LogOperationRequests = operationRequets;
            LogOperationResponses = operationResponses;
            LogEvents = events;
        }

        protected abstract void Initialize();

        private void OnPeerDisconnected(DisconnectReason disconnectReason, string s)
        {
            Peer.PeerDisconnectionNotifier.Disconnected -= OnPeerDisconnected;

            OperationRequestHandlerRegister?.Dispose();

            var ip = Peer.ConnectionInformation.Ip;
            var port = Peer.ConnectionInformation.Port;

            LogUtils.Log($"A peer {ip}:{port} has been disconnected. Reason: {disconnectReason} - Details: {s}");
        }
    }
}