using System.Collections.Generic;
using CommonTools.Log;
using Scripts.Containers.Service;
using Scripts.Gameplay.Actors.Entity;
using Shared.Game.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Containers.Entity
{
    public class EntityContainer : IEntityContainer
    {
        private const string ENTITIES_FOLDER_PATH = "Actors/{0}";

        private readonly Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

        private int localEntityId;

        public EntityContainer()
        {
            Debug.Log("EntityContainer");

            ServiceContainer.GameService.EntitiyInitialInformation.AddListener(AddLocalEntity);
            ServiceContainer.GameService.EntityAdded.AddListener(AddEntity);
            ServiceContainer.GameService.EntityRemoved.AddListener(RemoveEntity);
        }

        private void AddLocalEntity(EntityInitialInfomraitonEventParameters parameters)
        {
            Debug.Log("AddLocalEntity()");

            var entity = parameters.Entity;

            if (entities.ContainsKey(entity.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"Entity with id #{entity.Id} already exists."), LogMessageType.Warning);
                return;
            }

            var entityObjectName = entity.Type.ToString();

            var entityObject = CreateEntity($"Local {entityObjectName}");
            if (entityObject == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find an entity type - {entityObjectName}"), LogMessageType.Error);
                return;
            }

            var gameObject = Object.Instantiate(entityObject) as GameObject;
            entities.Add(entity.Id, gameObject.GetComponent<IEntity>());

            localEntityId = entity.Id;
        }

        private void AddEntity(EntityAddedEventParameters parameters)
        {
            foreach (var entity in parameters.Entity)
            {
                if (entities.ContainsKey(entity.Id))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Entity with id #{entity.Id} already exists."), LogMessageType.Warning);
                    continue;
                }

                var entityObjectName = entity.Type.ToString();

                var entityObject = CreateEntity(entityObjectName);
                if (entityObject == null)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find an entity type - {entityObjectName}"), LogMessageType.Error);
                    continue;
                }

                var gameObject = Object.Instantiate(entityObject) as GameObject;
                entities.Add(entity.Id, gameObject.GetComponent<IEntity>());                
            }
        }

        private void RemoveEntity(EntityRemovedEventParameters parameters)
        {
            foreach (var entityId in parameters.EntitiesId)
            {
                if (!entities.ContainsKey(entityId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find an entity with id #{entityId}"), LogMessageType.Warning);
                    continue;
                }

                entities.Remove(entityId);
            }
        }

        private Object CreateEntity(string entityObjectName)
        {
            var entityObject = Resources.Load(string.Format(ENTITIES_FOLDER_PATH, entityObjectName)).AssertNotNull();
            if (entityObject == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find an entity type - {entityObjectName}"), LogMessageType.Error);
                return null;
            }

            return entityObject;
        }

        public IEntity GetLocalEntity()
        {
            IEntity entity;
            if (entities.TryGetValue(localEntityId, out entity))
            {
                return entity;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a local entity with id #{localEntityId}"), LogMessageType.Warning);
            return null;
        }

        public IEntity GetRemoteEntity(int id)
        {
            IEntity entity;
            if (entities.TryGetValue(id, out entity))
            {
                return entity;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find an entity with id #{id}"), LogMessageType.Warning);
            return null;
        }
    }
}