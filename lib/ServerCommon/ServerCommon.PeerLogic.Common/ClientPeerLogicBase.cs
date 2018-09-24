using System;
using Common.ComponentModel;
using Common.Components;
using CommonTools.Log;
using ServerCommon.Application;
using ServerCommon.Application.Components;
using ServerCommon.Configuration;
using ServerCommon.PeerLogic.Components;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic.Common
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
        protected IExposedComponentsProvider ServerComponents =>
            ServerExposedComponents.Provide();

        protected IOperationRequestHandlerRegister<TOperationCode> OperationHandlerRegister
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public override void OnSetup()
        {
            AddCommonComponents();

            OperationHandlerRegister = ProvideOperationHandlerRegister();
        }

        /// <inheritdoc />
        public override void OnCleanup()
        {
            OperationHandlerRegister?.Dispose();
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