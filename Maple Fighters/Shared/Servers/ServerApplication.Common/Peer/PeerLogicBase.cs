using System;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerApplication.Common.Components;
using ServerApplication.Common.Components.Coroutines;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.PeerLogic
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
            EventSender = new EventSender<TEventCode>(PeerWrapper.Peer.EventSender, false);

            AddCommonComponents();

            var coroutinesExecutor = Entity.Container.GetComponent<CoroutinesExecutorEntity>() as ICoroutinesExecutor;

            OperationRequestHandlerRegister = new OperationRequestsHandler<TOperationCode>(PeerWrapper.Peer.OperationRequestNotifier, 
                PeerWrapper.Peer.OperationResponseSender, false, false, coroutinesExecutor);

            PeerWrapper.Peer.NetworkTrafficState = NetworkTrafficState.Flowing;
        }

        public virtual void Dispose()
        {
            Entity?.Dispose();
            OperationRequestHandlerRegister?.Dispose();
        }

        private void AddCommonComponents()
        {
            Entity.Container.AddComponent(new EventSenderWrapper(EventSender.AssertNotNull()));
            Entity.Container.AddComponent(new CoroutinesExecutorEntity(new FiberCoroutinesExecutor(PeerWrapper.Peer.Fiber, 100)));
        }
    }
}