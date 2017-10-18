using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using MathematicsHelper;
using PeerLogic.Common;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Components
{
    internal class PositionChangesListener : Component<IPeerEntity>
    {
        private CharacterSceneObjectGetter sceneObjectGetter;
        private InterestAreaManagement interestAreaManagement;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneObjectGetter = Entity.Container.GetComponent<CharacterSceneObjectGetter>().AssertNotNull();
            interestAreaManagement = Entity.Container.GetComponent<InterestAreaManagement>().AssertNotNull();

            SubscribeToPositionChangedEvent();
        }

        private void SubscribeToPositionChangedEvent()
        {
            var transform = sceneObjectGetter.GetSceneObject().Container.GetComponent<Transform>().AssertNotNull();
            transform.PositionAndDirectionChanged += SendNewPosition;
        }

        public void SendNewPosition(Vector2 newPosition, Directions direction)
        {
            var sceneObjectId = sceneObjectGetter.GetSceneObject().Id;
            var parameters = new SceneObjectPositionChangedEventParameters(sceneObjectId, newPosition.X, newPosition.Y, direction);
            var messageSendOptions = MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position);
            interestAreaManagement.SendEventForSubscribers((byte)GameEvents.PositionChanged, parameters, messageSendOptions);
        }
    }
}