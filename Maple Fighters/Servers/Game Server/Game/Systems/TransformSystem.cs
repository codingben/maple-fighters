using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.Application.Components;
using Game.Entities;
using Game.Entity.Components;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using ServerApplication.Common.Components.Coroutines;
using Shared.Game.Common;

namespace Game.Systems
{
    /*
    internal class TransformSystem
    {
        public readonly Action<IEntity> AddEntity;
        public readonly Action<IEntity> RemoveEntity;

        private readonly PeerContainer peerContainer;
        private readonly EntityIdToPeerIdConverter entityIdToPeerIdConverter;

        private readonly Dictionary<int, Transform> entitiesTransforms = new Dictionary<int, Transform>();

        public TransformSystem()
        {
            peerContainer = ServerComponents.Container.GetComponent<PeerContainer>().AssertNotNull();
            entityIdToPeerIdConverter = ServerComponents.Container.GetComponent<EntityIdToPeerIdConverter>().AssertNotNull();

            AddEntity = OnEntityAdded;
            RemoveEntity = OnEntityRemoved;

            var coroutinesExecuter = ServerComponents.Container.GetComponent<CoroutinesExecutor>().AssertNotNull();
            coroutinesExecuter.StartTask(EntitiesIteration);
        }

        private void OnEntityAdded(IEntity entity)
        {
            var transform = entity.Components.GetComponent<Transform>().AssertNotNull();
            if (transform == null)
            {
                return;
            }

            entitiesTransforms.Add(entity.Id, transform);
        }

        private void OnEntityRemoved(IEntity entity)
        {
            entitiesTransforms.Remove(entity.Id);
        }

        private async Task EntitiesIteration(IYield yield)
        {
            await yield.Return(new Execute(() =>
            {
                if (entitiesTransforms.Count > 0)
                {
                    PositionChangesNotifierIntervally();
                }
            }).Every(new WaitForSeconds(0.1f))
              .Until(new Forever()));
        }

        private void PositionChangesNotifierIntervally()
        {
            foreach (var transform in entitiesTransforms.Values)
            {
                if (Vector2.Distance(transform.NewPosition, transform.LastPosition) < 0.1f)
                {
                    continue;
                }

                SendNewPositionOfEntity(transform.OwnerEntity, transform.NewPosition);

                transform.LastPosition = transform.NewPosition;
            }
        }
        
        private void SendNewPositionOfEntity(IEntity entity, Vector2 newPosition)
        {
            foreach (var otherEntity in GetEntitiesFromEntityRegions(entity))
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

        private IEnumerable<IEntity> GetEntitiesFromEntityRegions(IEntity entity)
        {
            var publisherRegions = (entity.Components.GetComponent<InterestArea>().AssertNotNull() as IInterestArea)?.GetPublishers();
            var entities = new List<IEntity>();

            foreach (var publisherRegion in publisherRegions)
            {
                entities.AddRange(publisherRegion.GetAllSubscribers().Where(subscriber => subscriber.Id != entity.Id));
            }

            return entities.ToArray();
        }
    }
    */
}