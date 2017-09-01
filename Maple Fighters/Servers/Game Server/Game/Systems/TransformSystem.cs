using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class TransformSystem : Component
    {
        public readonly Action<IEntity> AddEntity;
        public readonly Action<IEntity> RemoveEntity;

        private readonly CoroutinesExecuter coroutinesExecuter;
        private ICoroutine entitiesIteratorCoroutine;

        private readonly PeerContainer peerContainer;
        private readonly EntityIdToPeerIdConverter entityIdToPeerIdConverter;

        private readonly Dictionary<int, Transform> entitiesTransforms = new Dictionary<int, Transform>();

        public TransformSystem()
        {
            coroutinesExecuter = ServerComponents.Container.GetComponent<CoroutinesExecuter>().AssertNotNull() as CoroutinesExecuter;

            peerContainer = ServerComponents.Container.GetComponent<PeerContainer>().AssertNotNull() as PeerContainer;
            entityIdToPeerIdConverter = ServerComponents.Container.GetComponent<EntityIdToPeerIdConverter>().AssertNotNull() as EntityIdToPeerIdConverter;

            AddEntity = OnEntityAdded;
            RemoveEntity = OnEntityRemoved;
        }

        private void OnEntityAdded(IEntity entity)
        {
            var transform = (Transform)entity.Components.GetComponent<Transform>().AssertNotNull();
            if (transform == null)
            {
                return;
            }

            entitiesTransforms.Add(entity.Id, transform);

            CheckEntitiesIteration();
        }

        private void OnEntityRemoved(IEntity entity)
        {
            entitiesTransforms.Remove(entity.Id);

            CheckEntitiesIteration();
        }

        private void CheckEntitiesIteration()
        {
            if (entitiesTransforms.Count > 0)
            {
                LogUtils.Log("CheckEntitiesIteration");

                entitiesIteratorCoroutine = coroutinesExecuter.StartCoroutine(EntitiesIteration());
            }
            else
            {
                entitiesIteratorCoroutine?.Dispose();
            }
        }

        private IEnumerator<IYieldInstruction> EntitiesIteration()
        {
            LogUtils.Log("EntitiesIteration");

            while (true)
            {
                PositionChangesNotifier();
                yield return null;
            }
        }

        private void PositionChangesNotifier()
        {
            LogUtils.Log("PositionChangesNotifier");

            foreach (var transform in entitiesTransforms.Values)
            {
                if (Vector2.Distance(transform.NewPosition, transform.LastPosition) < 1)
                {
                    continue;
                }

                SendNewPosition(transform.OwnerEntity, transform.NewPosition);

                transform.LastPosition = transform.NewPosition;
            }
        }

        private void SendNewPosition(IEntity ignoredEntity, Vector2 newPosition)
        {
            var ignoredEntityInterestArea = ignoredEntity.Components.GetComponent<InterestArea>().AssertNotNull() as IInterestArea;

            var publisherRegions = ignoredEntityInterestArea?.GetPublishers();

            var entities = new List<IEntity>();

            foreach (var publisherRegion in publisherRegions)
            {
                entities.AddRange(publisherRegion.GetAllSubscribers().Where(subscriber => subscriber.Id != ignoredEntity.Id));
            }

            // var entities = publisherRegions?.ConvertRegionsFromMatrix().Except(new[] { ignoredEntity });

            foreach (var entity in entities)
            {
                var peerId = entityIdToPeerIdConverter.GetPeerId(entity.Id);
                if (peerId == -1)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a peer id of an entity #{entity.Id}"));
                    continue;
                }

                var peerWrappper = peerContainer.GetPeerWrapper(peerId).AssertNotNull();
                if (peerWrappper == null)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a peer wrapper of an entity id #{entity.Id}"));
                    continue;
                }

                LogUtils.Log($"Entity Id: {ignoredEntity.Id} Changed Position to: {newPosition}");
                // MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position))
                var parameters = new EntityPositionChangedEventParameters(ignoredEntity.Id, newPosition.X, newPosition.Y);
                peerWrappper.SendEvent((byte)GameEvents.EntityPositionChanged, parameters, MessageSendOptions.DefaultReliable());
            }
        }
    }
}