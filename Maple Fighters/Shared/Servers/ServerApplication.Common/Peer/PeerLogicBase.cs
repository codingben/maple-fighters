using System;
using CommonCommunicationInterfaces;
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
        protected IPeerEntity Entity { get; } = new PeerEntity();
        protected IClientPeerWrapper<IClientPeer> PeerWrapper { get; private set; }
        protected IOperationRequestHandlerRegister<TOperationCode> OperationRequestHandlerRegister { get; private set; }
        protected IEventSender<TEventCode> EventSender { get; private set; }

        public virtual void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            PeerWrapper = peer;

            OperationRequestHandlerRegister = new OperationRequestsHandler<TOperationCode>(PeerWrapper.Peer.OperationRequestNotifier, 
                PeerWrapper.Peer.OperationResponseSender, false, false);
            EventSender = new EventSender<TEventCode>(PeerWrapper.Peer.EventSender, true);

            PeerWrapper.Peer.NetworkTrafficState = NetworkTrafficState.Flowing;
        }

        public void Dispose()
        {
            Entity?.Dispose();
            OperationRequestHandlerRegister?.Dispose();
        }

        protected void AddCommonComponents()
        {
            Entity.Container.AddComponent(new EventSenderWrapper(EventSender.AssertNotNull()));
            Entity.Container.AddComponent(new CoroutinesExecutorEntity(new FiberCoroutinesExecutor(PeerWrapper.Peer.Fiber, 100)));
        }
    }
}