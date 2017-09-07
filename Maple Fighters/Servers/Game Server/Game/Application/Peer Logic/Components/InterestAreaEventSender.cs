using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic.Components
{
    using Entity = Shared.Game.Common.Entity;

    public class InterestAreaEventSender : Component<IPeerEntity>
    {
        private EventSenderWrapper eventSender;
        private readonly InterestArea interestArea;

        public InterestAreaEventSender(InterestArea interestArea)
        {
            this.interestArea = interestArea;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            eventSender = Entity.Components.GetComponent<EventSenderWrapper>().AssertNotNull();

            interestArea.GameObjectAdded += OnGameObjectAdded;
            interestArea.GameObjectRemoved += OnGameObjectRemoved;
            interestArea.GameObjectsAdded += OnGameObjectsAdded;
            interestArea.GameObjectsRemoved += OnGameObjectsRemoved;
        }

        private void OnGameObjectAdded(IGameObject gameObject)
        {
            var entityTemp = new Entity(gameObject.Id, EntityType.Player);
            var parameters = new EntityAddedEventParameters(entityTemp);

            LogUtils.Log(MessageBuilder.Trace($"New gameobject - {gameObject.Id}"));

            eventSender.SendEvent((byte)GameEvents.EntityAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectRemoved(int gameObjectId)
        {
            var parameters = new EntityRemovedEventParameters(gameObjectId);
            LogUtils.Log(MessageBuilder.Trace($"Removed gameobject - {gameObjectId}"));
            eventSender.SendEvent((byte)GameEvents.EntityRemoved, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectsAdded(IGameObject[] gameObjects)
        {
            var entitiesTemp = new Entity[gameObjects.Length];
            for (var i = 0; i < entitiesTemp.Length; i++)
            {
                entitiesTemp[i].Id = gameObjects[i].Id;
                entitiesTemp[i].Type = EntityType.Player;
            }

            LogUtils.Log(MessageBuilder.Trace($"GameObjects Length - {gameObjects.Length}"));

            eventSender.SendEvent((byte)GameEvents.EntitiesAdded, new EntitiesAddedEventParameters(entitiesTemp), MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectsRemoved(int[] gameObjectsId)
        {
            var entitiesIdsTemp = new int[gameObjectsId.Length];
            for (var i = 0; i < entitiesIdsTemp.Length; i++)
            {
                entitiesIdsTemp[i] = gameObjectsId[i];
            }

            LogUtils.Log(MessageBuilder.Trace($"GameObjects Length - {gameObjectsId.Length}"));

            eventSender.SendEvent((byte)GameEvents.EntitiesRemoved, new EntitiesRemovedEventParameters(entitiesIdsTemp), MessageSendOptions.DefaultReliable());
        }
    }
}
