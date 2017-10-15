using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.GameObjects.Components;
using Game.InterestManagement;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;
using GameObject = Shared.Game.Common.GameObject;

namespace Game.Application.PeerLogic.Components
{
    internal class InterestAreaManagement : Component<IPeerEntity>
    {
        private PeerContainer peerContainer;
        private MinimalPeerGetter peerGetter;
        private EventSenderWrapper eventSender;
        private CharacterGameObjectGetter gameObjectGetter;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = Server.Entity.Container.GetComponent<PeerContainer>().AssertNotNull();

            eventSender = Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();
            gameObjectGetter = Entity.Container.GetComponent<CharacterGameObjectGetter>().AssertNotNull();
            peerGetter = Entity.Container.GetComponent<MinimalPeerGetter>().AssertNotNull();

            SubscribeToInterestAreaEvents();
        }

        private void SubscribeToInterestAreaEvents()
        {
            var interestArea = gameObjectGetter.GetGameObject().Container.GetComponent<InterestArea>().AssertNotNull();
            interestArea.SubscriberAdded = OnSubscriberAdded;
            interestArea.SubscriberRemoved = OnSubscriberRemoved;
            interestArea.SubscribersAdded = OnSubscribersAdded;
            interestArea.SubscribersRemoved = OnSubscribersRemoved;
            interestArea.DetectOverlapsWithRegionsAction.Invoke();
        }

        private void OnSubscriberAdded(InterestArea subscriber)
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var transform = subscriber.Entity.Container.GetComponent<Transform>().AssertNotNull();
            var gameObject = new GameObject(subscriber.Entity.Id, subscriber.Entity.Name, transform.Position.X, transform.Position.Y);

            var characterInformation = GetCharacterInformation(subscriber.Entity);
            var parameters = new GameObjectAddedEventParameters(gameObject, characterInformation.GetValueOrDefault(), characterInformation.HasValue);
            eventSender.Send((byte)GameEvents.GameObjectAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscriberRemoved(int subscriberId)
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var parameters = new GameObjectRemovedEventParameters(subscriberId);
            eventSender.Send((byte)GameEvents.GameObjectRemoved, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscribersAdded(IReadOnlyList<InterestArea> subscribers)
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var gameObjects = new GameObject[subscribers.Count];
            for (var i = 0; i < gameObjects.Length; i++)
            {
                var transform = subscribers[i].Entity.Container.GetComponent<Transform>().AssertNotNull();

                gameObjects[i].Id = subscribers[i].Entity.Id;
                gameObjects[i].Name = subscribers[i].Entity.Name;
                gameObjects[i].X = transform.Position.X;
                gameObjects[i].Y = transform.Position.Y;
            }

            eventSender.Send((byte)GameEvents.GameObjectsAdded, new GameObjectsAddedEventParameters(gameObjects, GetCharacterInformations(subscribers)), 
                MessageSendOptions.DefaultReliable());
        }

        private void OnSubscribersRemoved(IReadOnlyList<int> subscribersId)
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var gameObjectsId = new int[subscribersId.Count];
            for (var i = 0; i < gameObjectsId.Length; i++)
            {
                gameObjectsId[i] = subscribersId[i];
            }

            eventSender.Send((byte)GameEvents.GameObjectsRemoved, new GameObjectsRemovedEventParameters(gameObjectsId), 
                MessageSendOptions.DefaultReliable());
        }

        public void SendEventForSubscribers<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            foreach (var subscriber in GetSubscribersFromPublishers)
            {
                var peerId = subscriber.Entity.Container.GetComponent<PeerIdGetter>();
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
                eventSender.Send(code, parameters, messageSendOptions);
            }
        }

        private IEnumerable<InterestArea> GetSubscribersFromPublishers
        {
            get
            {
                var subscribers = new List<InterestArea>();
                var gameObject = gameObjectGetter.GetGameObject();

                var interestArea = gameObject.Container.GetComponent<InterestArea>().AssertNotNull();
                if (interestArea == null)
                {
                    return subscribers.ToArray();
                }

                foreach (var publisher in interestArea.GetPublishers())
                {
                    subscribers.AddRange(publisher.GetAllSubscribersArea().Where(subscriber => subscriber.Entity.Id != gameObject.Id));
                }
                return subscribers.ToArray();
            }
        }

        private CharacterInformation? GetCharacterInformation(IGameObject gameObject)
        {
            var characterInformationProvider = gameObject.Container.GetComponent<CharacterInformationProvider>();
            if (characterInformationProvider == null)
            {
                return null;
            }
            return new CharacterInformation(gameObject.Id, 
                characterInformationProvider.GetCharacterName(), characterInformationProvider.GetCharacterClass());
        }

        private CharacterInformation[] GetCharacterInformations(IEnumerable<InterestArea> subscribers)
        {
            var characterInformations = new List<CharacterInformation>();

            foreach (var gameObject in subscribers)
            {
                var characterInformationProvider = gameObject.Entity.Container.GetComponent<CharacterInformationProvider>();
                if (characterInformationProvider == null)
                {
                    continue;
                }

                characterInformations.Add(new CharacterInformation(gameObject.Entity.Id,
                    characterInformationProvider.GetCharacterName(), characterInformationProvider.GetCharacterClass()));
            }
            return characterInformations.ToArray();
        }
    }
}