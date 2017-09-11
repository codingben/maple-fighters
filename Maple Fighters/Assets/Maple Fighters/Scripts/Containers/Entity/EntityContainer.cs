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
            ServiceContainer.GameService.EntitiyInitialInformation.AddListener(AddLocalEntity);
            ServiceContainer.GameService.EntityAdded.AddListener(AddEntity);
            ServiceContainer.GameService.EntityRemoved.AddListener(RemoveEntity);
            ServiceContainer.GameService.EntitiesAdded.AddListener(AddEntities);
            ServiceContainer.GameService.EntitiesRemoved.AddListener(RemoveEntities);
        }

        private void AddLocalEntity(EnterWorldOperationResponseParameters parameters)
        {
            LogUtils.Log(MessageBuilder.Trace($"Local Id: {parameters.Entity.Id}"));

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

            var position = new Vector3(parameters.X, parameters.Y);
            LogUtils.Log(MessageBuilder.Trace($"Entity Id: {entity.Id} Starting Position: {position}"));
            var gameObject = Object.Instantiate(entityObject, position, Quaternion.identity) as GameObject;
            if (gameObject == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not instantiate an object {entityObjectName}"));
                return;
            }

            entities.Add(entity.Id, gameObject.GetComponent<IEntity>());

            localEntityId = entity.Id;
        }

        private void AddEntity(EntityAddedEventParameters parameters)
        {
            var entityId = parameters.Entity.Id;
            var entityType = parameters.Entity.Type;

            if (entities.ContainsKey(entityId))
            {
                LogUtils.Log(MessageBuilder.Trace($"Entity with id #{entityId} already exists."), LogMessageType.Warning);
                return;
            }

            var entityObjectName = entityType.ToString();

            var entityObject = CreateEntity(entityObjectName);
            if (entityObject == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find an entity type - {entityObjectName}"), LogMessageType.Error);
                return;
            }

            var position = new Vector3(parameters.Entity.X, parameters.Entity.Y);
            LogUtils.Log(MessageBuilder.Trace($"Entity Id: {entityId} Starting Position: {position}"));
            var gameObject = Object.Instantiate(entityObject, position, Quaternion.identity) as GameObject;
            if (gameObject == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not instantiate an object {entityObjectName}"));
                return;
            }

            entities.Add(entityId, gameObject.GetComponent<IEntity>());                
        }

        private void AddEntities(EntitiesAddedEventParameters parameters)
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

                var position = new Vector3(entity.X, entity.Y);
                LogUtils.Log(MessageBuilder.Trace($"Entity Id: {entity.Id} Starting Position: {position}"));
                var gameObject = Object.Instantiate(entityObject, position, Quaternion.identity) as GameObject;
                if (gameObject == null)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not instantiate an object {entityObjectName}"));
                    return;
                }

                entities.Add(entity.Id, gameObject.GetComponent<IEntity>());
            }
        }

        private void RemoveEntity(EntityRemovedEventParameters parameters)
        {
            var entityId = parameters.EntityId;

            if (!entities.ContainsKey(entityId))
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find an entity with id #{entityId}"), LogMessageType.Warning);
                return;
            }

            Object.Destroy(entities[entityId].GameObject);

            entities.Remove(entityId);
        }

        private void RemoveEntities(EntitiesRemovedEventParameters parameters)
        {
            foreach (var entityId in parameters.EntitiesId)
            {
                if (!entities.ContainsKey(entityId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find an entity with id #{entityId}"), LogMessageType.Warning);
                    continue;
                }

                Object.Destroy(entities[entityId].GameObject);

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