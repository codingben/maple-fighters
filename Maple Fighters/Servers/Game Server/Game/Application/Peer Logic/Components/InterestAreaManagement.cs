using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using ServerCommunicationInterfaces;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic.Components
{
    internal class InterestAreaManagement : Component<IPeerEntity>
    {
        private readonly IMinimalPeer peer;
        private readonly IGameObject gameObject;
        private readonly InterestArea interestArea;

        private PeerContainer peerContainer;
        private EventSenderWrapper eventSender;

        public InterestAreaManagement(IGameObject gameObject, IMinimalPeer peer)
        {
            this.gameObject = gameObject;
            this.peer = peer;

            interestArea = gameObject.AssertNotNull().Container.GetComponent<InterestArea>().AssertNotNull();
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = Server.Entity.Container.GetComponent<PeerContainer>().AssertNotNull();
            eventSender = Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();

            interestArea.GameObjectAdded = OnGameObjectAdded;
            interestArea.GameObjectRemoved = OnGameObjectRemoved;
            interestArea.GameObjectsAdded = OnGameObjectsAdded;
            interestArea.GameObjectsRemoved = OnGameObjectsRemoved;
        }

        private void OnGameObjectAdded(IGameObject gameObject)
        {
            if (!peer.IsConnected)
            {
                return;
            }

            var transform = gameObject.Container.GetComponent<Transform>().AssertNotNull();
            var entityTemp = new Entity(gameObject.Id, EntityType.Player, transform.Position.X, transform.Position.Y);
            var parameters = new EntityAddedEventParameters(entityTemp);

            eventSender.SendEvent((byte)GameEvents.EntityAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectRemoved(int gameObjectId)
        {
            if (!peer.IsConnected)
            {
                return;
            }

            var parameters = new EntityRemovedEventParameters(gameObjectId);
            eventSender.SendEvent((byte)GameEvents.EntityRemoved, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectsAdded(IGameObject[] gameObjects)
        {
            if (!peer.IsConnected)
            {
                return;
            }

            var entitiesTemp = new Entity[gameObjects.Length];
            for (var i = 0; i < entitiesTemp.Length; i++)
            {
                var transform = gameObjects[i].Container.GetComponent<Transform>().AssertNotNull();

                entitiesTemp[i].Id = gameObjects[i].Id;
                entitiesTemp[i].Type = EntityType.Player;
                entitiesTemp[i].X = transform.Position.X;
                entitiesTemp[i].Y = transform.Position.Y;
            }

            eventSender.SendEvent((byte)GameEvents.EntitiesAdded, new EntitiesAddedEventParameters(entitiesTemp), MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectsRemoved(int[] gameObjectsId)
        {
            if (!peer.IsConnected)
            {
                return;
            }

            var entitiesIdsTemp = new int[gameObjectsId.Length];
            for (var i = 0; i < entitiesIdsTemp.Length; i++)
            {
                entitiesIdsTemp[i] = gameObjectsId[i];
            }

            eventSender.SendEvent((byte)GameEvents.EntitiesRemoved, new EntitiesRemovedEventParameters(entitiesIdsTemp), MessageSendOptions.DefaultReliable());
        }

        public void SendEventOnlyForEntitiesInMyRegions<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            if (!peer.IsConnected)
            {
                return;
            }

            foreach (var otherEntity in GetEntitiesFromEntityRegions())
            {
                var peerWrapper = peerContainer.GetPeerWrapper(otherEntity.Id).AssertNotNull();
                var eventSender = peerWrapper?.PeerLogic.Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();

                eventSender?.SendEvent(code, parameters, messageSendOptions);
            }
        }

        public IEnumerable<IGameObject> GetEntitiesFromEntityRegions()
        {
            var gameObjects = new List<IGameObject>();

            if (interestArea == null)
            {
                return gameObjects.ToArray();
            }

            foreach (var publisherRegion in interestArea.GetPublishers())
            {
                gameObjects.AddRange(publisherRegion.GetAllSubscribers().Where(subscriber => subscriber.Id != gameObject.Id));
            }
            return gameObjects.ToArray();
        }
    }
}