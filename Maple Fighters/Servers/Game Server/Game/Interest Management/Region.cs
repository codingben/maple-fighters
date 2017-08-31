using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Entities;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;

namespace Game.InterestManagement
{
    internal class Region : IRegion
    {
        public Rectangle Rectangle { get; }

        private readonly Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

        private readonly PeerContainer peerContainer;
        private readonly EntityIdToPeerIdConverter entityIdToPeerIdConverter;

        public Region(Rectangle rectangle)
        {
            Rectangle = rectangle;

            peerContainer = ServerComponents.Container.GetComponent<PeerContainer>().AssertNotNull() as PeerContainer;
            entityIdToPeerIdConverter = ServerComponents.Container.GetComponent<EntityIdToPeerIdConverter>().AssertNotNull() as EntityIdToPeerIdConverter;
        }

        public void AddSubscription(IEntity entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Region::AddEntity() -> An entity with a id #{entity.Id} already exists in a region.", LogMessageType.Warning);
                return;
            }

            AddEntitiesForEntity(entity, entities.Values.ToArray());

            entities.Add(entity.Id, entity);
        }

        public void RemoveSubscription(IEntity entity)
        {
            if (!entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Region::RemoveEntity() -> An entity with a id #{entity.Id} does not exists in a region.", LogMessageType.Warning);
                return;
            }

            entities.Remove(entity.Id);

            RemoveEntitiesForEntity(entity, entities.Values.ToArray());
        }

        public bool HasSubscription(int entityId)
        {
            return entities.ContainsKey(entityId);
        }

        public List<IEntity> GetAllSubscribers()
        {
            return entities.Select(entity => entity.Value).ToList();
        }

        public void AddEntitiesForEntity(IEntity entity, IEntity[] entities)
        {
            var peerId = entityIdToPeerIdConverter.GetPeerId(entity.Id);

            var peerWrappper = peerContainer.GetPeerWrapper(peerId).AssertNotNull();
            if (peerWrappper == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find a peer wrapper of an entity id #{entity.Id}"));
                return;
            }

            var entitiesTemp = new Shared.Game.Common.Entity[entities.Length];
            for (var i = 0; i < entitiesTemp.Length; i++)
            {
                entitiesTemp[i].Id = entities[i].Id;
                entitiesTemp[i].Type = entities[i].Type;
            }

            peerWrappper.SendEvent((byte)GameEvents.EntityAdded, new EntityAddedEventParameters(entitiesTemp), MessageSendOptions.DefaultReliable());
        }

        public void RemoveEntitiesForEntity(IEntity entity, IEntity[] entities)
        {
            var peerId = entityIdToPeerIdConverter.GetPeerId(entity.Id);

            var peerWrappper = peerContainer.GetPeerWrapper(peerId).AssertNotNull();
            if (peerWrappper == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find a peer wrapper of an entity id #{entity.Id}"));
                return;
            }

            var entitiesIdTemp = new int[entities.Length];
            for (var i = 0; i < entitiesIdTemp.Length; i++)
            {
                entitiesIdTemp[i] = entities[i].Id;
            }

            peerWrappper.SendEvent((byte)GameEvents.EntityRemoved, new EntityRemovedEventParameters(entitiesIdTemp), MessageSendOptions.DefaultReliable());
        }
    }
}