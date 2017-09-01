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

            entities.Add(entity.Id, entity);

            AddEntitiesForEntity(entity, entities.Values.ToArray());
            AddEntityForEntities(entity);
        }

        private void AddEntityForEntities(IEntity newEntity)
        {
            foreach (var entity in entities)
            {
                if (entity.Key == newEntity.Id)
                {
                    continue;
                }

                var peerId = entityIdToPeerIdConverter.GetPeerId(entity.Key);
                if (peerId == -1)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a peer id via entity id #{entity.Key}"));
                    continue;
                }

                var peerWrappper = peerContainer.GetPeerWrapper(peerId).AssertNotNull();
                if (peerWrappper == null)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a peer wrapper of an entity id #{entity.Key}"));
                    continue;
                }

                var sharedEntity = new Shared.Game.Common.Entity
                {
                    Id = newEntity.Id,
                    Type = newEntity.Type
                };

                var parameters = new EntityAddedEventParameters(sharedEntity);
                peerWrappper.SendEvent((byte)GameEvents.EntityAdded, parameters, MessageSendOptions.DefaultReliable());
            }
        }

        private void RemoveEntityForEntities(int entityId)
        {
            foreach (var entity in entities)
            {
                if (entity.Key == entityId)
                {
                    continue;
                }

                var peerId = entityIdToPeerIdConverter.GetPeerId(entity.Key);
                if (peerId == -1)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a peer id via entity id #{entity.Key}"));
                    continue;
                }

                var peerWrappper = peerContainer.GetPeerWrapper(peerId).AssertNotNull();
                if (peerWrappper == null)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a peer wrapper of an entity id #{entity.Key}"));
                    continue;
                }

                var parameters = new EntityRemovedEventParameters(entityId);
                peerWrappper.SendEvent((byte)GameEvents.EntityRemoved, parameters, MessageSendOptions.DefaultReliable());
            }
        }

        public void RemoveSubscription(IEntity entity)
        {
            if (!entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Region::RemoveEntity() -> An entity with a id #{entity.Id} does not exists in a region.", LogMessageType.Warning);
                return;
            }

            entities.Remove(entity.Id);

            if (entities.Count > 0)
            {
                RemoveEntitiesForEntity(entity, entities.Values.ToArray());
            }

            RemoveEntityForEntities(entity.Id);
        }

        public bool HasSubscription(int entityId)
        {
            return entities.ContainsKey(entityId);
        }

        public List<IEntity> GetAllSubscribers()
        {
            return entities.Select(entity => entity.Value).ToList();
        }

        public void AddEntitiesForEntity(IEntity entity, IEnumerable<IEntity> entities)
        {
            var peerId = entityIdToPeerIdConverter.GetPeerId(entity.Id);
            if (peerId == -1)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find a peer id via entity id #{entity.Id}"));
                return;
            }

            var peerWrappper = peerContainer.GetPeerWrapper(peerId).AssertNotNull();
            if (peerWrappper == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find a peer wrapper of an entity id #{entity.Id}"));
                return;
            }

            var temp = entities.ToList();
            foreach (var entityTemp in temp)
            {
                if (entityTemp.Id != entity.Id)
                {
                    continue;
                }

                temp.Remove(entityTemp);
                break;
            }

            var entitiesTemp = new Shared.Game.Common.Entity[temp.Count];
            for (var i = 0; i < entitiesTemp.Length; i++)
            {
                entitiesTemp[i].Id = temp[i].Id;
                entitiesTemp[i].Type = temp[i].Type;
            }

            peerWrappper.SendEvent((byte)GameEvents.EntitiesAdded, new EntitiesAddedEventParameters(entitiesTemp), MessageSendOptions.DefaultReliable());
        }

        public void RemoveEntitiesForEntity(IEntity entity, IEnumerable<IEntity> entities)
        {
            var peerId = entityIdToPeerIdConverter.GetPeerId(entity.Id);
            if (peerId == -1)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find a peer id via entity id #{entity.Id}"));
                return;
            }

            var peerWrappper = peerContainer.GetPeerWrapper(peerId).AssertNotNull();
            if (peerWrappper == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find a peer wrapper of an entity id #{entity.Id}"));
                return;
            }

            var temp = entities.ToList();
            foreach (var entityTemp in temp)
            {
                if (entityTemp.Id != entity.Id)
                {
                    continue;
                }

                temp.Remove(entityTemp);
                break;
            }

            var entitiesIdsTemp = new int[temp.Count];
            for (var i = 0; i < entitiesIdsTemp.Length; i++)
            {
                entitiesIdsTemp[i] = temp[i].Id;
            }

            peerWrappper.SendEvent((byte)GameEvents.EntitiesRemoved, new EntitiesRemovedEventParameters(entitiesIdsTemp), MessageSendOptions.DefaultReliable());
        }
    }
}