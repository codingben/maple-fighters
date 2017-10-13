using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic.Components
{
    internal class PositionChangesListener : Component<IPeerEntity>
    {
        private CharacterGameObjectGetter gameObjectGetter;
        private InterestAreaManagement interestAreaManagement;

        protected override void OnAwake()
        {
            base.OnAwake();

            gameObjectGetter = Entity.Container.GetComponent<CharacterGameObjectGetter>().AssertNotNull();
            interestAreaManagement = Entity.Container.GetComponent<InterestAreaManagement>().AssertNotNull();

            SubscribeToPositionChangedEvent();
        }

        private void SubscribeToPositionChangedEvent()
        {
            var transform = gameObjectGetter.GetGameObject().Container.GetComponent<Transform>().AssertNotNull();
            transform.PositionAndDirectionChanged += SendNewPosition;
        }

        public void SendNewPosition(Vector2 newPosition, Directions direction)
        {
            var gameObjectId = gameObjectGetter.GetGameObject().Id;
            var parameters = new GameObjectPositionChangedEventParameters(gameObjectId, newPosition.X, newPosition.Y, direction);
            var messageSendOptions = MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position);
            interestAreaManagement.SendEventForSubscribers((byte)GameEvents.PositionChanged, parameters, messageSendOptions);
        }
    }
}