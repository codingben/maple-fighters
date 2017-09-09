using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic.Components
{
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

            eventSender = Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();

            interestArea.GameObjectAdded = OnGameObjectAdded;
            interestArea.GameObjectRemoved = OnGameObjectRemoved;
            interestArea.GameObjectsAdded = OnGameObjectsAdded;
            interestArea.GameObjectsRemoved = OnGameObjectsRemoved;
        }

        private void OnGameObjectAdded(IGameObject gameObject)
        {
            LogUtils.Log(MessageBuilder.Trace($"GameObject Id: {gameObject.Id}"));

            var entityTemp = new Entity(gameObject.Id, EntityType.Player);
            var parameters = new EntityAddedEventParameters(entityTemp);

            eventSender.SendEvent((byte)GameEvents.EntityAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectRemoved(int gameObjectId)
        {
            LogUtils.Log(MessageBuilder.Trace($"GameObject Id: {gameObjectId}"));

            var parameters = new EntityRemovedEventParameters(gameObjectId);
            eventSender.SendEvent((byte)GameEvents.EntityRemoved, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectsAdded(IGameObject[] gameObjects)
        {
            LogUtils.Log(MessageBuilder.Trace($"GameObjects Length: {gameObjects.Length}"));

            var entitiesTemp = new Entity[gameObjects.Length];
            for (var i = 0; i < entitiesTemp.Length; i++)
            {
                entitiesTemp[i].Id = gameObjects[i].Id;
                entitiesTemp[i].Type = EntityType.Player;
            }

            eventSender.SendEvent((byte)GameEvents.EntitiesAdded, new EntitiesAddedEventParameters(entitiesTemp), MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectsRemoved(int[] gameObjectsId)
        {
            LogUtils.Log(MessageBuilder.Trace($"GameObjects Length: {gameObjectsId.Length}"));

            var entitiesIdsTemp = new int[gameObjectsId.Length];
            for (var i = 0; i < entitiesIdsTemp.Length; i++)
            {
                entitiesIdsTemp[i] = gameObjectsId[i];
            }

            eventSender.SendEvent((byte)GameEvents.EntitiesRemoved, new EntitiesRemovedEventParameters(entitiesIdsTemp), MessageSendOptions.DefaultReliable());
        }
    }
}
