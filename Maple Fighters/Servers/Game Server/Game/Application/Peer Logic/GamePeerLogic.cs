using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Operations;
using Game.Entity.Components;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.Peer;
using Shared.Game.Common;

namespace Game.Application.PeerLogic
{
    internal class GamePeerLogic : PeerLogicWrapper<GameOperations, GameEvents>
    {
        private EntityWrapper entityWrapper;
        private readonly SceneContainer sceneContainer;

        public GamePeerLogic(IClientPeer peer, int peerId)
            : base(peer, peerId)
        {
            AddCommonComponents();

            sceneContainer = ServerComponents.Container.GetComponent<SceneContainer>().AssertNotNull();

            CreatePlayerEntity();

            HandleUpdateEntityPositionOperation();
            HandleEnterWorldOperation();
        }

        private void HandleUpdateEntityPositionOperation()
        {
            var transform = entityWrapper.Entity.Components.GetComponent<Transform>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.UpdateEntityPosition, new UpdateEntityPositionOperation(transform));
        }

        private void HandleEnterWorldOperation()
        {
            var transform = entityWrapper.Entity.Components.GetComponent<Transform>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.EnterWorld, new EnterWorldOperation(PeerId, entityWrapper.Entity.Id, transform));
        }

        private void CreatePlayerEntity()
        {
            entityWrapper = new EntityWrapper(EntityType.Player, PeerId);

            var entity = entityWrapper.Entity;
            entity.PresenceSceneId = 1;

            entity.Components.AddComponent(new Transform(entity, new Vector2(3.55f, 0.74f)));
            entity.Components.AddComponent(new InterestArea(entity, new Vector2(5, 2.5f)));
            entity.Components.AddComponent(new PositionEventSender(entity));

            sceneContainer.GetScene(1).AddEntity(entity);
        }

        protected override void OnPeerDisconnected(DisconnectReason disconnectReason, string s)
        {
            sceneContainer.GetScene(1).RemoveEntity(entityWrapper.Entity);

            entityWrapper.Dispose();

            base.OnPeerDisconnected(disconnectReason, s);
        }
    }
}