using System;
using CommonCommunicationInterfaces;
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
        public IEntity Entity { get; private set; }

        protected IClientPeerWrapper<IClientPeer> PeerWrapper { get; private set; }

        protected IOperationRequestHandlerRegister<TOperationCode> OperationRequestHandlerRegister { get; private set; }
        protected IEventSender<TEventCode> EventSender { get; private set; }

        public virtual void Initialize(IClientPeerWrapper<IClientPeer> peer, int peerId)
        {
            PeerWrapper = peer;

            OperationRequestHandlerRegister = new OperationRequestsHandler<TOperationCode>(PeerWrapper.Peer.OperationRequestNotifier, 
                PeerWrapper.Peer.OperationResponseSender, true, true);
            EventSender = new EventSender<TEventCode>(PeerWrapper.Peer.EventSender, true);

            Entity = new EntityWrapper(peerId);

            PeerWrapper.Peer.NetworkTrafficState = NetworkTrafficState.Flowing;
        }

        public void Dispose()
        {
            Entity?.Dispose();
            OperationRequestHandlerRegister?.Dispose();
        }

        protected void AddCommonComponents()
        {
            Entity.Components.AddComponent(new EventSender(EventSender));
            Entity.Components.AddComponent(new CoroutinesExecutorEntity(new FiberCoroutinesExecutor(PeerWrapper.Peer.Fiber, 100)));
        }
    }
}