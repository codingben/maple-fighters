using System;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Operations;
using Game.Entity.Components;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components.Coroutines;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.Peer;
using Shared.Game.Common;

namespace Game.Application.PeerLogic
{
    internal class PeerLogic : PeerLogicEntity<GameOperations, GameEvents>
    {
        private EntityWrapper entityWrapper;
        private readonly SceneContainer sceneContainer;

        public PeerLogic(IClientPeer peer, int peerId)
            : base(peer, peerId)
        {
            sceneContainer = ServerComponents.Container.GetComponent<SceneContainer>().AssertNotNull() as SceneContainer;

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
            entity.PresenceSceneId = 1;

            entity.Components.AddComponent(new CoroutinesExecuter(new FiberCoroutinesExecuter(Peer.Fiber, 100)));
            entity.Components.AddComponent(new Transform(entity));
            entity.Components.AddComponent(new InterestArea(entity, new Vector2(5, 2.5f),
                entity.Components.GetComponent<CoroutinesExecuter>().AssertNotNull() as ICoroutinesExecuter));

            sceneContainer.GetScene(1).AddEntity(entity);
        }

        protected override void OnPeerDisconnected(DisconnectReason disconnectReason, string s)
        {
            sceneContainer.GetScene(1).RemoveEntity(entityWrapper.Entity);

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