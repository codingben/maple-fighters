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

        private readonly ICoroutinesExecuter coroutinesExecuter;
        private ICoroutine entitiesIteratorCoroutine;

        private readonly PeerContainer peerContainer;
        private readonly EntityIdToPeerIdConverter entityIdToPeerIdConverter;

        private readonly Dictionary<int, Transform> entitiesTransforms = new Dictionary<int, Transform>();

        public TransformSystem()
        {
            coroutinesExecuter = ServerComponents.Container.GetComponent<CoroutinesExecuter>() as ICoroutinesExecuter;

            peerContainer = ServerComponents.Container.GetComponent<PeerContainer>().AssertNotNull() as PeerContainer;
            entityIdToPeerIdConverter = ServerComponents.Container.GetComponent<EntityIdToPeerIdConverter>().AssertNotNull() as EntityIdToPeerIdConverter;

            AddEntity = OnEntityAdded;
            RemoveEntity = OnEntityRemoved;
        }

        private void OnEntityAdded(IEntity entity)
        {
            if (!(entity.Components.GetComponent<Transform>().AssertNotNull() is Transform transform))
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
                entitiesIteratorCoroutine = coroutinesExecuter.StartCoroutine(EntitiesIteration());
            }
            else
            {
                entitiesIteratorCoroutine?.Dispose();
            }
        }

        private IEnumerator<IYieldInstruction> EntitiesIteration()
        {
            while (true)
            {
                PositionChangesNotifier();
                yield return null;
            }
        }

        private void PositionChangesNotifier()
        {
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
            var publisherRegion = ignoredEntity.Components.GetComponent<InterestArea>().AssertNotNull() as IInterestArea;
            var entities = publisherRegion?.GetPublishers();
            var otherEntities = entities?.ConvertRegionsFromMatrix().Except(new[] { ignoredEntity });

            foreach (var entity in otherEntities)
            {
                var peerId = entityIdToPeerIdConverter.GetPeerId(entity.Id);

                var peerWrappper = peerContainer.GetPeerWrapper(peerId).AssertNotNull();
                if (peerWrappper == null)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a peer wrapper of an entity id #{entity.Id}"));
                    continue;
                }

                peerWrappper.SendEvent(
                    (byte)GameEvents.EntityPositionChanged, 
                    new EntityPositionChangedEventParameters(ignoredEntity.Id, newPosition.X, newPosition.Y), 
                    new MessageSendOptions(true, (byte)GameDataChannels.Position, false, false)
                );
            }
        }
    }
}