using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Region : IRegion
    {
        public Rectangle Area { get; }

        private readonly Dictionary<int, IGameObject> gameObjects = new Dictionary<int, IGameObject>();

        public Region(Rectangle rectangle)
        {
            Area = rectangle;
        }

        public void AddSubscription(IGameObject gameObject)
        {
            if (gameObjects.ContainsKey(gameObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{gameObject.Id} already exists in a region."), LogMessageType.Error);
                return;
            }

            gameObjects.Add(gameObject.Id, gameObject);

            ShowGameObjectsForGameObject(gameObject);
            // AddEntityForEntities(gameObject);
        }

        public void RemoveSubscription(IGameObject gameObject)
        {
            if (!gameObjects.ContainsKey(gameObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game object with id #{gameObject.Id} does not exists in a region."), LogMessageType.Error);
                return;
            }

            gameObjects.Remove(gameObject.Id);

            /*if (gameObjects.Count > 0)
            {
                RemoveEntitiesForEntity(gameObject, gameObjects.Values.ToArray());
            }

            RemoveEntityForEntities(gameObject.Id);*/
        }

        public bool HasSubscription(int gameObjectId)
        {
            return gameObjects.ContainsKey(gameObjectId);
        }

        public IEnumerable<IGameObject> GetAllSubscribers()
        {
            return gameObjects.Select(gameObject => gameObject.Value).ToList();
        }

        private void ShowGameObjectsForGameObject(IGameObject gameObject)
        {
            /*var peerId = entityIdToPeerIdConverter.GetPeerId(entity.Id);
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

            peerWrappper.SendEvent((byte)GameEvents.EntitiesAdded, new EntitiesAddedEventParameters(entitiesTemp), MessageSendOptions.DefaultReliable());*/
        }

        /*private void RemoveEntitiesForEntity(IGameObject entity, IEnumerable<IGameObject> entities)
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
        }*/

        /*private void AddEntityForEntities(IGameObject newGameObject)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Key == newGameObject.Id)
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
        }*/

        /*private void RemoveEntityForEntities(int entityId)
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
        }*/
    }
}