using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic.Components
{
    internal class InterestAreaManagement : Component<IPeerEntity>
    {
        private PeerContainer peerContainer;
        private EventSenderWrapper eventSender;
        private GameObjectGetter gameObjectGetter;
        private MinimalPeerGetter minimalPeerGetter;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = Server.Entity.Container.GetComponent<PeerContainer>().AssertNotNull();

            eventSender = Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();
            gameObjectGetter = Entity.Container.GetComponent<GameObjectGetter>().AssertNotNull();
            minimalPeerGetter = Entity.Container.GetComponent<MinimalPeerGetter>().AssertNotNull();

            SubscribeToInterestAreaEvents();
        }

        private void SubscribeToInterestAreaEvents()
        {
            var interestArea = gameObjectGetter.GetGameObject().Container.GetComponent<InterestArea>().AssertNotNull();
            interestArea.GameObjectAdded = OnGameObjectAdded;
            interestArea.GameObjectRemoved = OnGameObjectRemoved;
            interestArea.GameObjectsAdded = OnGameObjectsAdded;
            interestArea.GameObjectsRemoved = OnGameObjectsRemoved;
            interestArea.DetectOverlapsWithRegionsAction.Invoke();
        }

        private void OnGameObjectAdded(InterestArea gameObject)
        {
            if (!minimalPeerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var transform = gameObject.Entity.Container.GetComponent<Transform>().AssertNotNull();
            var gameObjectTemp = new Shared.Game.Common.GameObject(gameObject.Entity.Id, gameObject.Entity.Name, transform.Position.X, transform.Position.Y);

            var parameters = new GameObjectAddedEventParameters(gameObjectTemp);
            eventSender.SendEvent((byte)GameEvents.GameObjectAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectRemoved(int gameObjectId)
        {
            if (!minimalPeerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var parameters = new GameObjectRemovedEventParameters(gameObjectId);
            eventSender.SendEvent((byte)GameEvents.GameObjectRemoved, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectsAdded(InterestArea[] gameObjects)
        {
            if (!minimalPeerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var gameObjectsTemp = new Shared.Game.Common.GameObject[gameObjects.Length];
            for (var i = 0; i < gameObjectsTemp.Length; i++)
            {
                var transform = gameObjects[i].Entity.Container.GetComponent<Transform>().AssertNotNull();

                gameObjectsTemp[i].Id = gameObjects[i].Entity.Id;
                gameObjectsTemp[i].Name = gameObjects[i].Entity.Name;
                gameObjectsTemp[i].X = transform.Position.X;
                gameObjectsTemp[i].Y = transform.Position.Y;
            }

            eventSender.SendEvent((byte)GameEvents.GameObjectsAdded, new GameObjectsAddedEventParameters(gameObjectsTemp), MessageSendOptions.DefaultReliable());
        }

        private void OnGameObjectsRemoved(int[] gameObjectsId)
        {
            if (!minimalPeerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var gameObjectsIdTemp = new int[gameObjectsId.Length];
            for (var i = 0; i < gameObjectsIdTemp.Length; i++)
            {
                gameObjectsIdTemp[i] = gameObjectsId[i];
            }

            eventSender.SendEvent((byte)GameEvents.GameObjectsRemoved, new GameObjectsRemovedEventParameters(gameObjectsIdTemp), MessageSendOptions.DefaultReliable());
        }

        public void SendEventForGameObjectsInMyRegions<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            if (!minimalPeerGetter.GetPeer().IsConnected)
            {
                return;
            }

            foreach (var gameObject in GetGameObjectsFromEntityRegions())
            {
                var peerId = gameObject.Entity.Container.GetComponent<PeerIdGetter>();
                if (peerId == null)
                {
                    continue;
                }

                var peerWrapper = peerContainer.GetPeerWrapper(peerId.GetId());
                if (peerWrapper == null)
                {
                    continue;
                }

                var eventSender = peerWrapper.PeerLogic.Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();
                eventSender.SendEvent(code, parameters, messageSendOptions);
            }
        }

        public IEnumerable<InterestArea> GetGameObjectsFromEntityRegions()
        {
            var gameObjects = new List<InterestArea>();

            var gameObject = gameObjectGetter.GetGameObject();
            var interestArea = gameObject.Container.GetComponent<InterestArea>().AssertNotNull();

            if (interestArea == null)
            {
                return gameObjects.ToArray();
            }

            foreach (var publisher in interestArea.GetPublishers())
            {
                gameObjects.AddRange(publisher.GetAllSubscribers().Where(subscriber => subscriber.Entity.Id != gameObject.Id));
            }
            return gameObjects.ToArray();
        }
    }
}