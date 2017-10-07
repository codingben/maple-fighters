using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Components;
using Game.Application.PeerLogic.Operations;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.PeerLogic;
using Shared.Game.Common;

namespace Game.Application.PeerLogic
{
    internal class AuthenticatedGamePeerLogic : PeerLogicBase<GameOperations, GameEvents>
    {
        private readonly IGameObject gameObject;

        public AuthenticatedGamePeerLogic()
        {
            gameObject = CreatePlayerGameObject();
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            SubscribeToDisconnectedEvent();

            AddComponents();

            AddHandlerForEnterWorldOperation();
            AddHandlerForUpdateEntityPositionOperation();
            AddHandlerForUpdatePlayerStateOperation();
            AddHandlerForChangeSceneOperation();
        }

        private IGameObject CreatePlayerGameObject()
        {
            var playerGameObjectCreator = Server.Entity.Container.GetComponent<PlayerGameObjectCreator>().AssertNotNull();
            var playerGameObject = playerGameObjectCreator.Create(Maps.Map_1, new Vector2(10, -5.5f));
            playerGameObject.Container.AddComponent(new PeerIdGetter(PeerWrapper.PeerId));
            return playerGameObject;
        }

        private void AddComponents()
        {
            Entity.Container.AddComponent(new GameObjectGetter(gameObject));
            Entity.Container.AddComponent(new MinimalPeerGetter(PeerWrapper.Peer));
            Entity.Container.AddComponent(new InterestAreaManagement());
            Entity.Container.AddComponent(new PositionChangesListener());
        }

        private void AddHandlerForEnterWorldOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.EnterWorld, new EnterWorldOperationHandler(gameObject));
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
            var gameObjectGetter = Entity.Container.GetComponent<GameObjectGetter>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.ChangeScene, new ChangeSceneOperationHandler(gameObjectGetter));
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