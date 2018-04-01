using System;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common.Components;
using ServerCommunicationHelper;
using JsonConfig;

namespace PeerLogic.Common
{
    public abstract class PeerLogicBase<TOperationCode, TEventCode> : IPeerLogicBase
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        public IContainer Components { get; } = new Container();

        protected IClientPeerWrapper ClientPeerWrapper { get; private set; }
        protected IOperationRequestHandlerRegister<TOperationCode> OperationHandlerRegister { get; private set; }
        private IEventSender<TEventCode> EventSender { get; set; }

        public virtual void Initialize(IClientPeerWrapper clientPeerWrapper)
        {
            ClientPeerWrapper = clientPeerWrapper;

            AddEventsSenderHandler();
            AddOperationRequestsHandler();
        }

        public virtual void Dispose()
        {
            Components?.Dispose();
            OperationHandlerRegister?.Dispose();
        }

        private void AddEventsSenderHandler()
        {
            var logEvents = (bool)Config.Global.Log.Events;
            EventSender = new EventSender<TEventCode>(ClientPeerWrapper.Peer.EventSender, logEvents);
        }

        private void AddOperationRequestsHandler()
        {
            var logOperationsRequest = (bool)Config.Global.Log.OperationsRequest;
            var logOperationsResponse = (bool)Config.Global.Log.OperationsResponse;

            // A coroutines executor is necessary to handle async operation handlers
            var coroutinesExecutor = Components.AddComponent(new CoroutinesExecutor(new FiberCoroutinesExecutor(ClientPeerWrapper.Peer.Fiber, 100)));

            OperationHandlerRegister = new OperationRequestsHandler<TOperationCode>(ClientPeerWrapper.Peer.OperationRequestNotifier,
                ClientPeerWrapper.Peer.OperationResponseSender, logOperationsRequest, logOperationsResponse, coroutinesExecutor);
        }

        protected void AddCommonComponents()
        {
            Components.AddComponent(new MinimalPeerGetter(ClientPeerWrapper.PeerId, ClientPeerWrapper.Peer));
            Components.AddComponent(new ClientPeerGetter(ClientPeerWrapper.PeerId, ClientPeerWrapper.Peer));
            Components.AddComponent(new EventSenderWrapper(EventSender.AssertNotNull()));
        }
    }
}