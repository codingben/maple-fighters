using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Components;
using Game.Application.PeerLogic.Operations;
using Game.InterestManagement;
using PeerLogic.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;
using Shared.Game.Common;

namespace Game.Application.PeerLogics
{
    internal class GameScenePeerLogic : PeerLogicBase<GameOperations, GameEvents>
    {
        private readonly ISceneObject characterSceneObject;
        private readonly Character character;

        public GameScenePeerLogic(Character character)
        {
            this.character = character;

            characterSceneObject = CreateCharacterSceneObject(character);
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            SubscribeToDisconnectedEvent();

            AddComponents();

            AddHandlerForEnterSceneOperation();
            AddHandlerForUpdatePositionOperation();
            AddHandlerForUpdatePlayerStateOperation();
            AddHandlerForChangeSceneOperation();
        }

        private void AddComponents()
        {
            characterSceneObject.Container.AddComponent(new PeerIdGetter(PeerWrapper.PeerId));

            Entity.Container.AddComponent(new CharacterGetter(characterSceneObject, character));
            Entity.Container.AddComponent(new MinimalPeerGetter(PeerWrapper.Peer));
            Entity.Container.AddComponent(new InterestAreaManagement());
            Entity.Container.AddComponent(new PositionChangesListener());
        }

        private void AddHandlerForEnterSceneOperation()
        {
            var characterGetter = Entity.Container.GetComponent<ICharacterGetter>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.EnterScene, new EnterSceneOperationHandler(characterGetter));
        }

        private void AddHandlerForUpdatePositionOperation()
        {
            var transform = characterSceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.PositionChanged, new UpdatePositionOperationHandler(transform));
        }

        private void AddHandlerForUpdatePlayerStateOperation()
        {
            var interestAreaManagement = Entity.Container.GetComponent<IInterestAreaManagement>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.PlayerStateChanged, 
                new UpdatePlayerStateOperationHandler(characterSceneObject.Id, interestAreaManagement));
        }

        private void AddHandlerForChangeSceneOperation()
        {
            var characterGetter = Entity.Container.GetComponent<ICharacterGetter>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.ChangeScene, new ChangeSceneOperationHandler(characterGetter));
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
            characterSceneObject.Dispose();

            UnsubscribeFromDisconnectedEvent();
        }

        private ISceneObject CreateCharacterSceneObject(Character character)
        {
            var characterSceneObjectCreator = Server.Entity.Container.GetComponent<ICharacterCreator>().AssertNotNull();
            var characterSceneObject = characterSceneObjectCreator.Create(character);
            return characterSceneObject;
        }
    }
}