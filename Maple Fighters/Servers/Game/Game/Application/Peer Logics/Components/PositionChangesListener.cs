using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using MathematicsHelper;
using PeerLogic.Common;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Components
{
    internal class PositionChangesListener : Component<IPeerEntity>
    {
        private ICharacterGetter sceneObjectGetter;
        private IInterestAreaNotifier interestAreaNotifier;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneObjectGetter = Entity.Container.GetComponent<ICharacterGetter>().AssertNotNull();
            interestAreaNotifier = sceneObjectGetter.GetSceneObject().Container.GetComponent<IInterestAreaNotifier>().AssertNotNull();

            var transform = sceneObjectGetter.GetSceneObject().Container.GetComponent<ITransform>().AssertNotNull();
            transform.PositionDirectionChanged += SendPosition;
        }

        private void SendPosition(Vector2 newPosition, Directions direction)
        {
            var sceneObjectId = sceneObjectGetter.GetSceneObject().Id;
            var parameters = new SceneObjectPositionChangedEventParameters(sceneObjectId, newPosition.X, newPosition.Y, direction);
            var messageSendOptions = MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position);
            interestAreaNotifier.NotifySubscribers((byte)GameEvents.PositionChanged, parameters, messageSendOptions);
        }
    }
}