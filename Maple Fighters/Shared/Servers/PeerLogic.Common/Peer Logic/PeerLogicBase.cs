using System;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common.Components;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;
using JsonConfig;

namespace PeerLogic.Common
{
    public abstract class PeerLogicBase<TOperationCode, TEventCode> : IPeerLogicBase
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        public IContainer Components { get; } = new Container();

        protected IClientPeerWrapper<IClientPeer> PeerWrapper { get; private set; }
        protected IOperationRequestHandlerRegister<TOperationCode> OperationHandlerRegister { get; private set; }
        private IEventSender<TEventCode> EventSender { get; set; }

        public virtual void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            PeerWrapper = peer;

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
            EventSender = new EventSender<TEventCode>(PeerWrapper.Peer.EventSender, logEvents);
        }

        private void AddOperationRequestsHandler()
        {
            var logOperationsRequest = (bool)Config.Global.Log.OperationsRequest;
            var logOperationsResponse = (bool)Config.Global.Log.OperationsResponse;

            // A coroutines executor is necessary to handle async operation handlers
            var coroutinesExecutor = Components.AddComponent(new CoroutinesExecutor(new FiberCoroutinesExecutor(PeerWrapper.Peer.Fiber, 100)));

            OperationHandlerRegister = new OperationRequestsHandler<TOperationCode>(PeerWrapper.Peer.OperationRequestNotifier,
                PeerWrapper.Peer.OperationResponseSender, logOperationsRequest, logOperationsResponse, coroutinesExecutor);
        }

        protected void AddCommonComponents()
        {
            Components.AddComponent(new MinimalPeerGetter(PeerWrapper.PeerId, PeerWrapper.Peer));
            Components.AddComponent(new EventSenderWrapper(EventSender.AssertNotNull()));
        }
    }
}