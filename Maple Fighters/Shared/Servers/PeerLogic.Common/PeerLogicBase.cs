using System;
using CommonTools.Log;
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
        public IPeerEntity Entity { get; private set; }

        protected IClientPeerWrapper<IClientPeer> PeerWrapper { get; private set; }
        protected IOperationRequestHandlerRegister<TOperationCode> OperationRequestHandlerRegister { get; private set; }
        protected IEventSender<TEventCode> EventSender { get; private set; }

        public virtual void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            PeerWrapper = peer;
            Entity = new PeerEntity(PeerWrapper.PeerId);

            AddEventsSenderHandler();
            AddOperationRequestsHandler();
        }

        public virtual void Dispose()
        {
            Entity?.Dispose();
            OperationRequestHandlerRegister?.Dispose();
        }

        private void AddEventsSenderHandler()
        {
            var logEvents = Config.Global.Log.Events;
            EventSender = new EventSender<TEventCode>(PeerWrapper.Peer.EventSender, logEvents);
        }

        private void AddOperationRequestsHandler()
        {
            var logOperationsRequest = Config.Global.Log.OperationsRequest;
            var logOperationsResponse = Config.Global.Log.OperationsResponse;

            // Necessary for async operation handlers.
            var coroutinesExecutor = Entity.Container.AddComponent(new CoroutinesExecutor(new FiberCoroutinesExecutor(PeerWrapper.Peer.Fiber, 100)));

            OperationRequestHandlerRegister = new OperationRequestsHandler<TOperationCode>(PeerWrapper.Peer.OperationRequestNotifier,
                PeerWrapper.Peer.OperationResponseSender, logOperationsRequest, logOperationsResponse, coroutinesExecutor);
        }

        protected void AddCommonComponents()
        {
            Entity.Container.AddComponent(new MinimalPeerGetter(PeerWrapper.Peer));
            Entity.Container.AddComponent(new EventSenderWrapper(EventSender.AssertNotNull()));
        }
    }
}