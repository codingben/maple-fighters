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
        private ISceneObjectGetter sceneObjectGetter;
        private IInterestAreaNotifier interestAreaNotifier;
        private ITransform transform;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneObjectGetter = Components.GetComponent<ISceneObjectGetter>().AssertNotNull();

            interestAreaNotifier = sceneObjectGetter.GetSceneObject().Components.GetComponent<IInterestAreaNotifier>().AssertNotNull();
            transform = sceneObjectGetter.GetSceneObject().Components.GetComponent<ITransform>().AssertNotNull();
            transform.PositionChanged += UpdatePositionForAll;
        }

        private void UpdatePositionForAll(Vector2 newPosition)
        {
            var sceneObjectId = sceneObjectGetter.GetSceneObject().Id;
            var direction = transform.Direction.GetDirectionsFromDirection();

            var parameters = new SceneObjectPositionChangedEventParameters(sceneObjectId, newPosition.X, newPosition.Y, direction);
            var messageSendOptions = MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position);
            interestAreaNotifier.NotifySubscribers((byte)GameEvents.PositionChanged, parameters, messageSendOptions);
        }
    }
}