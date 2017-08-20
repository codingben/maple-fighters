using System;
using CommonCommunicationInterfaces;
using Game.Application.Components;
using Game.Application.PeerLogic.Operations;
using Game.Entities;
using Game.Entity.Components;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.Peer;
using Shared.Game.Common;

namespace Game.Application.PeerLogic
{
    internal class PeerLogic : PeerLogicEntity<GameOperations, GameEvents>
    {
        private EntityWrapper entityWrapper;

        public PeerLogic(IClientPeer peer, int peerId)
            : base(peer, peerId)
        {
            SetOperationsHandlers();
            CreateEntity();
        }

        private void SetOperationsHandlers()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.Test, new TestOperation(EventSender));
        }

        private void CreateEntity()
        {
            entityWrapper = new EntityWrapper(EntityType.Player, PeerId);
            entityWrapper.Entity.Components.AddComponent(new Transform());
        }

        protected override void OnPeerDisconnected(DisconnectReason disconnectReason, string s)
        {
            entityWrapper.Dispose();

            base.OnPeerDisconnected(disconnectReason, s);
        }

        public override void SendEvent<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
        {
            var gameEvent = (GameEvents)Enum.ToObject(typeof(GameEvents), code);
            EventSender.Send(gameEvent, parameters, messageSendOptions);
        }
    }
}