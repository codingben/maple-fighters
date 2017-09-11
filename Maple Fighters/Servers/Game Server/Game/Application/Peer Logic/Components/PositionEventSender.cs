using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic.Components
{
    internal class PositionEventSender : Component<IPeerEntity>
    {
        private readonly IGameObject playerGameObject;
        private PeerContainer peerContainer;

        public PositionEventSender(IGameObject playerGameObject)
        {
            this.playerGameObject = playerGameObject;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = Server.Entity.Container.GetComponent<PeerContainer>().AssertNotNull();

            var transform = playerGameObject.Container.GetComponent<Transform>().AssertNotNull();
            transform.PositionChanged += SendNewPosition;
        }

        public void SendNewPosition(Vector2 newPosition)
        {
            foreach (var otherEntity in GetEntitiesFromEntityRegions())
            {
                if (playerGameObject.Id == otherEntity.Id)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Cannot send a new position to an entity id #{otherEntity.Id}"));
                    continue;
                }

                var peerWrapper = peerContainer.GetPeerWrapper(otherEntity.Id).AssertNotNull();

                var eventSender = peerWrapper?.PeerLogic.Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();
                if (eventSender == null)
                {
                    continue;
                }

                var parameters = new EntityPositionChangedEventParameters(playerGameObject.Id, newPosition.X, newPosition.Y);
                eventSender.SendEvent((byte)GameEvents.EntityPositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
            }
        }

        private IEnumerable<IGameObject> GetEntitiesFromEntityRegions()
        {
            var publisherRegions = (playerGameObject.Container.GetComponent<InterestArea>().AssertNotNull() as IInterestArea)?.GetPublishers();
            var gameObjects = new List<IGameObject>();

            if (publisherRegions == null)
            {
                return gameObjects.ToArray();
            }

            foreach (var publisherRegion in publisherRegions)
            {
                gameObjects.AddRange(publisherRegion.GetAllSubscribers().Where(subscriber => subscriber.Id != playerGameObject.Id));
            }
            return gameObjects.ToArray();
        }
    }
}