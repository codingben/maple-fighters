using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Entities;
using Game.Systems;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;

namespace Game.InterestManagement
{
    internal class Scene : IScene
    {
        public int SceneId { get; }

        public Rectangle Boundaries { get; }

        private readonly Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

        private readonly Region[,] regions;

        private readonly PeerContainer peerContainer;
        private readonly EntityIdToPeerIdConverter entityIdToPeerIdConverter;

        private readonly TransformSystem transformSystem;

        public Scene(Rectangle sceneSize, Vector2 regionSize, int sceneId)
        {
            Boundaries = sceneSize;
            SceneId = sceneId;

            // var sceneSize = new Vector2(10, 10);
            // var regionSize = new Vector2(1, 1);
            
            var regionsX = (int)(sceneSize.X / regionSize.X);
            var regionsY = (int)(sceneSize.Y / regionSize.Y);

            regions = new Region[regionsX, regionsY];

            var x = regionSize.X / 2;
            var y = regionSize.Y / 2;

            for (var i = 0; i < regions.GetLength(0); i++)
            {
                for (var j = 0; j < regions.GetLength(1); j++)
                {
                    var regionPositionX = x + (i * regionSize.X);
                    var regionPositionY = y + (j * regionSize.Y);

                    regions[i, j] = new Region(new Rectangle(new Vector2(regionPositionX, regionPositionY), new Vector2(regionSize.X, regionSize.Y)));

                    // regions[i, j].transform.position = new Vector3(x + (i * regionSize.x), y + (j * regionSize.y));
                    // regions[i, j].transform.localScale = new Vector3(regionSize.x, regionSize.y);
                }
            }

            peerContainer = ServerComponents.Container.GetComponent<PeerContainer>().AssertNotNull() as PeerContainer;
            entityIdToPeerIdConverter = ServerComponents.Container.GetComponent<EntityIdToPeerIdConverter>().AssertNotNull() as EntityIdToPeerIdConverter;

            transformSystem = new TransformSystem();
        }

        public void AddEntity(IEntity entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Scene::AddEntity() -> An entity with a id #{entity.Id} already exists in a scene.", LogMessageType.Warning);
                return;
            }

            entity.PresenceSceneId = SceneId;

            entities.Add(entity.Id, entity);

            transformSystem.AddEntity(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            if (!entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Scene::AddEntity() -> An entity with a id #{entity.Id} does not exists in a scene.", LogMessageType.Warning);
                return;
            }

            entity.PresenceSceneId = -1;

            entities.Remove(entity.Id);

            transformSystem.RemoveEntity(entity);
        }

        public IEntity GetEntity(int entityId)
        {
            if (entities.TryGetValue(entityId, out var entity))
            {
                return entity;
            }

            LogUtils.Log($"Scene::GetEntity() - Could not found an entity id #{entityId}", LogMessageType.Error);

            return null;
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

            var entitiesTemp = new Shared.Game.Common.Entity[entities.Length];
            for (var i = 0; i < entitiesTemp.Length; i++)
            {
                entitiesTemp[i].Id = entities[i].Id;
                entitiesTemp[i].Type = entities[i].Type;
            }

            peerWrappper.SendEvent((byte)GameEvents.EntityRemoved, new EntityRemovedEventParameters(entitiesTemp), MessageSendOptions.DefaultReliable());
        }
    }
}