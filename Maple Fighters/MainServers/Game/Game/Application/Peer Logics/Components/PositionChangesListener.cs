using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.PeerLogic.Components.Interfaces;
using Game.Application.GameObjects.Components.Interfaces;
using MathematicsHelper;
using Game.Common;
using InterestManagement.Components.Interfaces;

namespace Game.Application.PeerLogic.Components
{
    internal class PositionChangesListener : Component
    {
        private IPlayerGameObjectGetter playerGameObjectGetter;
        private IInterestAreaNotifier interestAreaNotifier;

        protected override void OnAwake()
        {
            base.OnAwake();

            playerGameObjectGetter = Components.GetComponent<IPlayerGameObjectGetter>().AssertNotNull();
            interestAreaNotifier = playerGameObjectGetter.GetPlayerGameObject().Components.GetComponent<IInterestAreaNotifier>().AssertNotNull();

            SubscribeToPositionChanged();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnubscribeFromPositionChanged();
        }

        private void SubscribeToPositionChanged()
        {
            var transform = playerGameObjectGetter.GetPlayerGameObject().Components.GetComponent<IPositionTransform>().AssertNotNull();
            transform.PositionChanged += UpdatePositionForOthers;
        }

        private void UnubscribeFromPositionChanged()
        {
            var transform = playerGameObjectGetter.GetPlayerGameObject().Components.GetComponent<IPositionTransform>().AssertNotNull();
            transform.PositionChanged -= UpdatePositionForOthers;
        }

        private void UpdatePositionForOthers(Vector2 position)
        {
            var id = playerGameObjectGetter.GetPlayerGameObject().Id;
            var directionTransform = playerGameObjectGetter.GetPlayerGameObject().Components.GetComponent<IDirectionTransform>().AssertNotNull();
            var direction = directionTransform.Direction.GetDirectionsFromDirection();
            var parameters = new SceneObjectPositionChangedEventParameters(id, position.X, position.Y, direction);
            interestAreaNotifier.NotifySubscribers((byte)GameEvents.PositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }
    }
}