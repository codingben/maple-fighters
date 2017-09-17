using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.PeerLogic.Components;
using Game.Application.PeerLogic.Operations;
using Game.InterestManagement;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.PeerLogic;
using Shared.Game.Common;

namespace Game.Application.PeerLogic
{
    internal class AuthenticatedGamePeerLogic : PeerLogicBase<GameOperations, GameEvents>
    {
        private readonly IGameObject gameObject;

        public AuthenticatedGamePeerLogic(IGameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            SubscribeToDisconnectedEvent();

            AddComponents();

            AddHandlerForUpdateEntityPositionOperation();
            AddHandlerForUpdatePlayerStateOperation();
            AddHandlerForChangeSceneOperation();
        }

        private void AddComponents()
        {
            Entity.Container.AddComponent(new InterestAreaManagement(gameObject, PeerWrapper.Peer));
            Entity.Container.AddComponent(new PositionChangesListener(gameObject));
        }

        private void AddHandlerForUpdateEntityPositionOperation()
        {
            var transform = gameObject.Container.GetComponent<Transform>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.PositionChanged, new UpdateEntityPositionOperationHandler(transform));
        }

        private void AddHandlerForUpdatePlayerStateOperation()
        {
            var interestAreaManagement = Entity.Container.GetComponent<InterestAreaManagement>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.PlayerStateChanged, new UpdatePlayerStateOperationHandler(gameObject.Id, interestAreaManagement));
        }

        private void AddHandlerForChangeSceneOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.ChangeScene, new ChangeSceneOperationHandler(gameObject));
        }

        private void SubscribeToDisconnectedEvent()
        {
            PeerWrapper.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectedEvent()
        {
            PeerWrapper.Disconnected -= OnDisconnected;
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            gameObject.Dispose();

            UnsubscribeFromDisconnectedEvent();
        }
    }
}