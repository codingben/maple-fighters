using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;
using SceneObject = Shared.Game.Common.SceneObject;

namespace Game.Application.PeerLogic.Components
{
    internal class InterestAreaManagement : Component<IPeerEntity>
    {
        private PeerContainer peerContainer;
        private MinimalPeerGetter peerGetter;
        private EventSenderWrapper eventSender;
        private CharacterSceneObjectGetter sceneObjectGetter;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = Server.Entity.Container.GetComponent<PeerContainer>().AssertNotNull();

            eventSender = Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();
            sceneObjectGetter = Entity.Container.GetComponent<CharacterSceneObjectGetter>().AssertNotNull();
            peerGetter = Entity.Container.GetComponent<MinimalPeerGetter>().AssertNotNull();

            SubscribeToInterestAreaEvents();
        }

        private void SubscribeToInterestAreaEvents()
        {
            var interestArea = sceneObjectGetter.GetSceneObject().Container.GetComponent<InterestArea>().AssertNotNull();
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
            var sceneObject = new SceneObject(subscriber.Entity.Id, subscriber.Entity.Name, transform.Position.X, transform.Position.Y);

            var characterInformation = GetCharacterInformation(subscriber.Entity);
            var parameters = new SceneObjectAddedEventParameters(sceneObject, characterInformation.GetValueOrDefault(), characterInformation.HasValue);
            eventSender.Send((byte)GameEvents.SceneObjectAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscriberRemoved(int subscriberId)
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var parameters = new SceneObjectRemovedEventParameters(subscriberId);
            eventSender.Send((byte)GameEvents.SceneObjectRemoved, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscribersAdded(IReadOnlyList<InterestArea> subscribers)
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var sceneObjects = new SceneObject[subscribers.Count];
            for (var i = 0; i < sceneObjects.Length; i++)
            {
                var transform = subscribers[i].Entity.Container.GetComponent<Transform>().AssertNotNull();

                sceneObjects[i].Id = subscribers[i].Entity.Id;
                sceneObjects[i].Name = subscribers[i].Entity.Name;
                sceneObjects[i].X = transform.Position.X;
                sceneObjects[i].Y = transform.Position.Y;
            }

            eventSender.Send((byte)GameEvents.SceneObjectsAdded, new SceneObjectsAddedEventParameters(sceneObjects, GetCharacterInformations(subscribers)), 
                MessageSendOptions.DefaultReliable());
        }

        private void OnSubscribersRemoved(IReadOnlyList<int> subscribersId)
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var sceneObjectsId = new int[subscribersId.Count];
            for (var i = 0; i < sceneObjectsId.Length; i++)
            {
                sceneObjectsId[i] = subscribersId[i];
            }

            eventSender.Send((byte)GameEvents.SceneObjectsRemoved, new SceneObjectsRemovedEventParameters(sceneObjectsId), 
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
                var sceneObject = sceneObjectGetter.GetSceneObject();

                var interestArea = sceneObject.Container.GetComponent<InterestArea>().AssertNotNull();
                if (interestArea == null)
                {
                    return subscribers.ToArray();
                }

                foreach (var publisher in interestArea.GetPublishers())
                {
                    subscribers.AddRange(publisher.GetAllSubscribersArea().Where(subscriber => subscriber.Entity.Id != sceneObject.Id));
                }
                return subscribers.ToArray();
            }
        }

        private CharacterInformation? GetCharacterInformation(ISceneObject sceneObject)
        {
            var characterInformationProvider = sceneObject.Container.GetComponent<CharacterInformationProvider>();
            if (characterInformationProvider == null)
            {
                return null;
            }
            return new CharacterInformation(sceneObject.Id, 
                characterInformationProvider.GetCharacterName(), characterInformationProvider.GetCharacterClass());
        }

        private CharacterInformation[] GetCharacterInformations(IEnumerable<InterestArea> subscribers)
        {
            var characterInformations = new List<CharacterInformation>();

            foreach (var sceneObject in subscribers)
            {
                var characterInformationProvider = sceneObject.Entity.Container.GetComponent<CharacterInformationProvider>();
                if (characterInformationProvider == null)
                {
                    continue;
                }

                characterInformations.Add(new CharacterInformation(sceneObject.Entity.Id,
                    characterInformationProvider.GetCharacterName(), characterInformationProvider.GetCharacterClass()));
            }
            return characterInformations.ToArray();
        }
    }
}