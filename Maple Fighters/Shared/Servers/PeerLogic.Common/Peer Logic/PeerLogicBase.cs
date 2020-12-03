using System;
using CommonTools.Log;
using ComponentModel.Common;
using Components.Common;
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

        public void Initialize(IClientPeerWrapper clientPeerWrapper)
        {
            ClientPeerWrapper = clientPeerWrapper;

            SetEventsSenderHandler();
            SetOperationRequestsHandler();

            OnInitialized();
        }

        protected abstract void OnInitialized();

        public void Dispose()
        {
            Components?.Dispose();
            OperationHandlerRegister?.Dispose();

            OnDispose();
        }

        protected virtual void OnDispose()
        {
            // Left blank intentionally
        }

        private void SetEventsSenderHandler()
        {
            var logEvents = (bool)Config.Global.Log.Events;
            EventSender = new EventSender<TEventCode>(ClientPeerWrapper.Peer.EventSender, logEvents);
        }

        private void SetOperationRequestsHandler()
        {
            var logOperationsRequest = (bool)Config.Global.Log.OperationsRequest;
            var logOperationsResponse = (bool)Config.Global.Log.OperationsResponse;
            var coroutinesManager = Components.AddComponent(new CoroutinesManager(new FiberCoroutinesExecutor(ClientPeerWrapper.Peer.Fiber, 100))); // A coroutines executor is necessary to handle async operation handlers
            OperationHandlerRegister = new OperationRequestsHandler<TOperationCode>(ClientPeerWrapper.Peer.OperationRequestNotifier, ClientPeerWrapper.Peer.OperationResponseSender, 
                logOperationsRequest, logOperationsResponse, coroutinesManager);
        }

        protected void AddCommonComponents()
        {
            Components.AddComponent(new ClientPeerProvider(ClientPeerWrapper.PeerId, ClientPeerWrapper.Peer));
            Components.AddComponent(new EventSenderWrapper(EventSender.AssertNotNull()));
        }
    }
}