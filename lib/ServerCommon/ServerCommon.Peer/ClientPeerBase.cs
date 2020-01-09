using System;
using CommonTools.Coroutines;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace ServerCommon.Peer
{
    public abstract class ClientPeerBase<TO, TE>
        where TO : IComparable, IFormattable, IConvertible
        where TE : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestHandlerRegister<TO> OperationHandlerRegister
        {
            get;
        }

        protected IEventSender<TE> EventSender
        {
            get;
        }

        protected IFiber Fiber
        {
            get;
        }

        protected ICoroutinesExecutor CoroutinesExecutor => GetCoroutinesExecutor();

        protected ClientPeerBase(IClientPeer peer, bool log = false)
        {
            OperationHandlerRegister = new OperationRequestsHandler<TO>(peer.OperationRequestNotifier, peer.OperationResponseSender, log, log, CoroutinesExecutor);
            EventSender = new EventSender<TE>(peer.EventSender, log);
            Fiber = peer.Fiber;
        }

        public void Dispose()
        {
            OperationHandlerRegister?.Dispose();
        }

        protected abstract ICoroutinesExecutor GetCoroutinesExecutor();
    }
}