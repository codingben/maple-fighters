using System;
using Common.Components;
using CommonTools.Log;
using ServerCommon.Application.Components;
using ServerCommon.Configuration;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic.Components
{
    /// <inheritdoc />
    /// <summary>
    /// A common implementation for the client peer logic which handles operations and events.
    /// </summary>
    /// <typeparam name="TOperationCode">The operations.</typeparam>
    /// <typeparam name="TEventCode">The events.</typeparam>
    public class ClientPeerLogicBase<TOperationCode, TEventCode> : PeerLogicBase<IClientPeer>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestHandlerRegister<TOperationCode> OperationHandlerRegister
        {
            get;
            private set;
        }

        protected override void Setup()
        {
            base.Setup();

            AddCommonComponents();

            OperationHandlerRegister = ProvideOperationHandlerRegister();

            OnSetup();
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            
            OperationHandlerRegister?.Dispose();

            OnCleanup();
        }

        protected virtual void OnSetup()
        {
            // Left blank intentionally
        }

        protected virtual void OnCleanup()
        {
            // Left blank intentionally
        }

        private void AddCommonComponents()
        {
            var executor = new FiberCoroutinesExecutor(
                Peer.Fiber,
                updateRateMilliseconds: 100);

            Components.Add(new CoroutinesManager(executor));
            Components.Add(new EventSenderProvider<TEventCode>(
                Peer, 
                Peer.EventSender));
        }

        private IOperationRequestHandlerRegister<TOperationCode> ProvideOperationHandlerRegister()
        {
            var coroutinesManager = Components.Get<ICoroutinesManager>().AssertNotNull();

            return new OperationRequestsHandler<TOperationCode>(
                Peer.OperationRequestNotifier,
                Peer.OperationResponseSender,
                ServerSettings.Peer.Operations.LogRequests,
                ServerSettings.Peer.Operations.LogResponses,
                coroutinesManager);
        }
    }
}