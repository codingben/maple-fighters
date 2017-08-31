using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Operations;
using Game.Entity.Components;
using Game.InterestManagement;
using MathematicsHelper;
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
            CreatePlayerEntity();
            SetOperationsHandlers();
        }

        private void SetOperationsHandlers()
        {
            var transform = entityWrapper.Entity.Components.GetComponent<Transform>().AssertNotNull() as Transform;
            OperationRequestHandlerRegister.SetHandler(GameOperations.UpdateEntityPosition, new UpdateEntityPositionOperation(transform));

            OperationRequestHandlerRegister.SetHandler(GameOperations.EnterWorld, new EnterWorldOperation(PeerId, entityWrapper.Entity.Id));
        }

        private void CreatePlayerEntity()
        {
            entityWrapper = new EntityWrapper(EntityType.Player, PeerId);

            var entity = entityWrapper.Entity;
            entity.Components.AddComponent(new Transform(entity));
            entity.Components.AddComponent(new InterestArea(entity, new Vector2(10, 10)));
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