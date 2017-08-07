using System;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using PhotonServerImplementation;
using ServerCommunicationHelper;

namespace Shared.Servers.Common
{
    public abstract class ClientPeerLogicBase<TOperationCode, TEventCode> : PhotonClientPeer
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestHandlerRegister<TOperationCode> OperationRequestHandlerRegister { get; private set; }
        protected new IEventSender<TEventCode> EventSender { get; private set; }

        protected bool LogOperationRequests;
        protected bool LogOperationResponses;
        protected bool LogEvents;

        protected ClientPeerLogicBase(InitRequest request) 
            : base(request)
        {
            InitializeCommunication();
        }

        private void InitializeCommunication()
        {
            OperationRequestHandlerRegister = new OperationRequestsHandler<TOperationCode>(
                OperationRequestNotifier,
                OperationResponseSender,
                LogOperationRequests,
                LogOperationResponses
            );

            EventSender = new EventSender<TEventCode>(
                base.EventSender,
                LogEvents
            );
        }

        protected void ActivateLogs(bool operationRequets, bool operationResponses, bool events)
        {
            LogOperationRequests = operationRequets;
            LogOperationResponses = operationResponses;
            LogEvents = events;
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            base.OnDisconnect(reasonCode, reasonDetail);

            OnDisconnected(reasonCode, reasonDetail);
        }

        protected abstract void OnDisconnected(DisconnectReason reasonCode, string reasonDetail);
    }
}