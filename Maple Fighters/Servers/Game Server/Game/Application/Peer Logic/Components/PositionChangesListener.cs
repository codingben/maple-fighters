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
        private InterestAreaManagement interestAreaManagement;
        private readonly IGameObject gameObject;

        public PositionChangesListener(IGameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            interestAreaManagement = Entity.Container.GetComponent<InterestAreaManagement>().AssertNotNull();

            SubscribeToPositionChangedEvent();
        }

        private void SubscribeToPositionChangedEvent()
        {
            var transform = gameObject.Container.GetComponent<Transform>().AssertNotNull();
            transform.PositionAndDirectionChanged += SendNewPosition;
        }

        public void SendNewPosition(Vector2 newPosition, Directions direction)
        {
            var parameters = new EntityPositionChangedEventParameters(gameObject.Id, newPosition.X, newPosition.Y, direction);
            interestAreaManagement?.SendEventOnlyForEntitiesInMyRegions((byte)GameEvents.EntityPositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }
    }
}