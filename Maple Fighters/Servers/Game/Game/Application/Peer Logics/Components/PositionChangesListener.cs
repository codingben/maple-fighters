using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using MathematicsHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Components
{
    internal class PositionChangesListener : Component
    {
        private ICharacterGetter sceneObjectGetter;
        private IInterestAreaNotifier interestAreaNotifier;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneObjectGetter = Entity.GetComponent<ICharacterGetter>().AssertNotNull();
            interestAreaNotifier = sceneObjectGetter.GetSceneObject().Container.GetComponent<IInterestAreaNotifier>().AssertNotNull();

            var transform = sceneObjectGetter.GetSceneObject().Container.GetComponent<ITransform>().AssertNotNull();
            transform.PositionChanged += UpdatePositionForAll;
        }

        private void UpdatePositionForAll(Vector2 newPosition, Directions direction)
        {
            var sceneObjectId = sceneObjectGetter.GetSceneObject().Id;
            var parameters = new SceneObjectPositionChangedEventParameters(sceneObjectId, newPosition.X, newPosition.Y, direction);
            var messageSendOptions = MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position);
            interestAreaNotifier.NotifySubscribers((byte)GameEvents.PositionChanged, parameters, messageSendOptions);
        }
    }
}