using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Entities;
using Game.Entity.Components;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class PositionEventSender : EntityComponent
    {
        private readonly Transform transform;

        private readonly PeerContainer peerContainer;
        private readonly EntityIdToPeerIdConverter entityIdToPeerIdConverter;

        public PositionEventSender(IEntity entity) 
            : base(entity)
        {
            transform = OwnerEntity.Components.GetComponent<Transform>().AssertNotNull();
            transform.PositionChanged += SendNewPosition;

            peerContainer = ServerComponents.Container.GetComponent<PeerContainer>().AssertNotNull();
            entityIdToPeerIdConverter = ServerComponents.Container.GetComponent<EntityIdToPeerIdConverter>().AssertNotNull();
        }

        public new void Dispose()
        {
            transform.PositionChanged -= SendNewPosition;
        }

        public void SendNewPosition(Vector2 newPosition)
        {
            foreach (var otherEntity in GetEntitiesFromEntityRegions())
            {
                var peerId = entityIdToPeerIdConverter.GetPeerId(otherEntity.Id);
                if (peerId == -1)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a peer id of an entity #{otherEntity.Id}"));
                    continue;
                }

                var peerWrappper = peerContainer.GetPeerWrapper(peerId).AssertNotNull();
                if (peerWrappper == null)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a peer wrapper of an entity id #{otherEntity.Id}"));
                    continue;
                }

                var parameters = new EntityPositionChangedEventParameters(otherEntity.Id, newPosition.X, newPosition.Y);
                peerWrappper.SendEvent((byte)GameEvents.EntityPositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
            }
        }

        private IEnumerable<IEntity> GetEntitiesFromEntityRegions()
        {
            var publisherRegions = (OwnerEntity.Components.GetComponent<InterestArea>().AssertNotNull() as IInterestArea)?.GetPublishers();
            var entities = new List<IEntity>();

            foreach (var publisherRegion in publisherRegions)
            {
                entities.AddRange(publisherRegion.GetAllSubscribers().Where(subscriber => subscriber.Id != OwnerEntity.Id));
            }

            return entities.ToArray();
        }
    }
}