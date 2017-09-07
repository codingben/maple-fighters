using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.Components
{
    internal class PositionEventSender : Component<IPeerEntity>
    {
        private readonly IGameObject playerGameObject;
        private EventSenderWrapper eventSender;

        public PositionEventSender(IGameObject playerGameObject)
        {
            this.playerGameObject = playerGameObject;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            eventSender = Entity.Components.GetComponent<EventSenderWrapper>().AssertNotNull();

            var transform = playerGameObject.Components.GetComponent<Transform>().AssertNotNull();
            transform.PositionChanged += SendNewPosition;
        }

        public void SendNewPosition(Vector2 newPosition)
        {
            foreach (var otherEntity in GetEntitiesFromEntityRegions())
            {
                var parameters = new EntityPositionChangedEventParameters(otherEntity.Id, newPosition.X, newPosition.Y);
                eventSender.SendEvent((byte)GameEvents.EntityPositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
            }
        }

        private IEnumerable<IGameObject> GetEntitiesFromEntityRegions()
        {
            var publisherRegions = (playerGameObject.Components.GetComponent<InterestArea>().AssertNotNull() as IInterestArea)?.GetPublishersExceptMyGameObject();
            var gameObjects = new List<IGameObject>();

            if (publisherRegions == null)
            {
                return gameObjects.ToArray();
            }

            foreach (var publisherRegion in publisherRegions)
            {
                gameObjects.AddRange(publisherRegion.GetAllSubscribers()
                    .Where(subscriber => subscriber.Id != playerGameObject.Id));
            }
            return gameObjects.ToArray();
        }
    }
}