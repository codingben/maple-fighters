using System;
using CommonTools.Coroutines;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace ServerCommon.Peer
{
    public abstract class ClientPeerBase<TO, TE> : IDisposable
        where TO : IComparable, IFormattable, IConvertible
        where TE : IComparable, IFormattable, IConvertible
    {
        protected IMinimalPeer Peer
        {
            get;
        }

        protected IOperationRequestHandlerRegister<TO> OperationHandlerRegister
        {
            get;
        }

        protected IEventSender<TE> EventSender
        {
            get;
        }

        protected ICoroutinesExecutor CoroutinesExecutor => GetCoroutinesExecutor();

        protected ClientPeerBase(IClientPeer peer, bool log = false)
        {
            Peer = peer;
            OperationHandlerRegister = new OperationRequestsHandler<TO>(
                peer.OperationRequestNotifier,
                peer.OperationResponseSender,
                log,
                log,
                CoroutinesExecutor);
            EventSender = new EventSender<TE>(peer.EventSender, log);
        }

        public void Dispose()
        {
            OperationHandlerRegister?.Dispose();
        }

        protected abstract ICoroutinesExecutor GetCoroutinesExecutor();
    }
}