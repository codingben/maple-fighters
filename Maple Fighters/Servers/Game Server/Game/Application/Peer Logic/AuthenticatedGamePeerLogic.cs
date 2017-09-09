using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
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
        private readonly IGameObject playerGameObject;

        public AuthenticatedGamePeerLogic(IGameObject playerGameObject)
        {
            this.playerGameObject = playerGameObject;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            SubscribeToDisconnectedEvent();

            AddCommonComponents();
            AddComponents();

            AddHandlerForUpdateEntityPosition();
        }

        private void AddComponents()
        {
            var interestArea = playerGameObject.Container.GetComponent<InterestArea>().AssertNotNull();
            Entity.Container.AddComponent(new InterestAreaEventSender(interestArea));
            Entity.Container.AddComponent(new PositionEventSender(playerGameObject));
        }

        private void AddHandlerForUpdateEntityPosition()
        {
            var transform = playerGameObject.Container.GetComponent<Transform>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(GameOperations.UpdateEntityPosition, new UpdateEntityPositionOperation(transform));
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
            playerGameObject.Dispose();

            UnsubscribeFromDisconnectedEvent();
        }
    }
}