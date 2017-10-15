using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Components;
using Game.Application.PeerLogic.Operations;
using Game.InterestManagement;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.PeerLogic;
using Shared.Game.Common;

namespace Game.Application.PeerLogics
{
    internal class AuthenticatedPeerLogic : PeerLogicBase<GameOperations, GameEvents>
    {
        private readonly IGameObject characterGameObject;
        private readonly Character character;

        public AuthenticatedPeerLogic(Character character)
        {
            this.character = character;
            this.characterGameObject = CreateCharacterGameObject(character);
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            SubscribeToDisconnectedEvent();

            AddComponents();

            AddHandlerForUpdatePositionOperation();
            AddHandlerForUpdatePlayerStateOperation();
            AddHandlerForChangeSceneOperation();
        }

        private void AddComponents()
        {
            characterGameObject.Container.AddComponent(new PeerIdGetter(PeerWrapper.PeerId));

            Entity.Container.AddComponent(new CharacterGameObjectGetter(characterGameObject, character));
            Entity.Container.AddComponent(new MinimalPeerGetter(PeerWrapper.Peer));
            Entity.Container.AddComponent(new InterestAreaManagement());
            Entity.Container.AddComponent(new PositionChangesListener());
            Entity.Container.AddComponent(new LocalGameObjectFetcher());
        }

        private void AddHandlerForUpdatePositionOperation()
        {
            var transform = characterGameObject.Container.GetComponent<Transform>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.PositionChanged, new UpdatePositionOperationHandler(transform));
        }

        private void AddHandlerForUpdatePlayerStateOperation()
        {
            var interestAreaManagement = Entity.Container.GetComponent<InterestAreaManagement>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.PlayerStateChanged, new UpdatePlayerStateOperationHandler(characterGameObject.Id, interestAreaManagement));
        }

        private void AddHandlerForChangeSceneOperation()
        {
            var gameObjectGetter = Entity.Container.GetComponent<CharacterGameObjectGetter>().AssertNotNull();
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
            characterGameObject.Dispose();

            UnsubscribeFromDisconnectedEvent();
        }

        private IGameObject CreateCharacterGameObject(Character character)
        {
            var characterGameObjectCreator = Server.Entity.Container.GetComponent<CharacterGameObjectCreator>().AssertNotNull();
            var characterGameObject = characterGameObjectCreator.Create(character);
            return characterGameObject;
        }
    }
}