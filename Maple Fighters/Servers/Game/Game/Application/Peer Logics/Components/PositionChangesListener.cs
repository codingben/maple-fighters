using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.PeerLogic.Components.Interfaces;
using Game.Application.SceneObjects.Components.Interfaces;
using MathematicsHelper;
using Game.Common;
using InterestManagement.Components.Interfaces;

namespace Game.Application.PeerLogic.Components
{
    internal class PositionChangesListener : Component
    {
        private ISceneObjectGetter sceneObjectGetter;
        private IInterestAreaNotifier interestAreaNotifier;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneObjectGetter = Components.GetComponent<ISceneObjectGetter>().AssertNotNull();
            interestAreaNotifier = sceneObjectGetter.GetSceneObject().Components.GetComponent<IInterestAreaNotifier>().AssertNotNull();

            SubscribeToPositionChanged();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnubscribeFromPositionChanged();
        }

        private void SubscribeToPositionChanged()
        {
            var transform = sceneObjectGetter.GetSceneObject().Components.GetComponent<IPositionTransform>().AssertNotNull();
            transform.PositionChanged += UpdatePositionForOthers;
        }

        private void UnubscribeFromPositionChanged()
        {
            var transform = sceneObjectGetter.GetSceneObject().Components.GetComponent<IPositionTransform>().AssertNotNull();
            transform.PositionChanged -= UpdatePositionForOthers;
        }

        private void UpdatePositionForOthers(Vector2 position)
        {
            var sceneObjectId = sceneObjectGetter.GetSceneObject().Id;
            var directionTransform = sceneObjectGetter.GetSceneObject().Components.GetComponent<IDirectionTransform>().AssertNotNull();
            var direction = directionTransform.Direction.GetDirectionsFromDirection();
            var parameters = new SceneObjectPositionChangedEventParameters(sceneObjectId, position.X, position.Y, direction);
            interestAreaNotifier.NotifySubscribers((byte)GameEvents.PositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }
    }
}