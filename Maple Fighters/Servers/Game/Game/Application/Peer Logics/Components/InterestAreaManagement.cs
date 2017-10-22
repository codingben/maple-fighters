using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using PeerLogic.Common;
using PeerLogic.Common.Components;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;
using Shared.Game.Common;
using SceneObject = Shared.Game.Common.SceneObject;

namespace Game.Application.PeerLogic.Components
{
    internal class InterestAreaManagement : Component<IPeerEntity>, IInterestAreaManagement
    {
        private IPeerContainer peerContainer;
        private IMinimalPeerGetter peerGetter;
        private IEventSenderWrapper eventSender;
        private ICharacterGetter sceneObjectGetter;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = Server.Entity.Container.GetComponent<IPeerContainer>().AssertNotNull();

            eventSender = Entity.Container.GetComponent<IEventSenderWrapper>().AssertNotNull();
            sceneObjectGetter = Entity.Container.GetComponent<ICharacterGetter>().AssertNotNull();
            peerGetter = Entity.Container.GetComponent<IMinimalPeerGetter>().AssertNotNull();

            SubscribeToInterestAreaEvents();
        }

        private void SubscribeToInterestAreaEvents()
        {
            var interestArea = sceneObjectGetter.GetSceneObject().Container.GetComponent<IInterestArea>().AssertNotNull();
            interestArea.SubscriberAdded += OnSubscriberAdded;
            interestArea.SubscriberRemoved += OnSubscriberRemoved;
            interestArea.SubscribersAdded += OnSubscribersAdded;
            interestArea.SubscribersRemoved += OnSubscribersRemoved;
            interestArea.DetectOverlapsWithRegions();
        }

        private void OnSubscriberAdded(ISceneObject sceneObject)
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var transform = sceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            var sharedSceneObject = new SceneObject(sceneObject.Id, sceneObject.Name, transform.Position.X, transform.Position.Y);

            var characterInformation = GetCharacterInformation(sceneObject);
            var parameters = new SceneObjectAddedEventParameters(sharedSceneObject, characterInformation.GetValueOrDefault(), characterInformation.HasValue);
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

        private void OnSubscribersAdded(IReadOnlyList<ISceneObject> sceneObjects)
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var sharedSceneObjects = new SceneObject[sceneObjects.Count];
            for (var i = 0; i < sceneObjects.Count; i++)
            {
                sharedSceneObjects[i].Id = sceneObjects[i].Id;
                sharedSceneObjects[i].Name = sceneObjects[i].Name;

                var transform = sceneObjects[i].Container.GetComponent<ITransform>().AssertNotNull();

                sharedSceneObjects[i].X = transform.Position.X;
                sharedSceneObjects[i].Y = transform.Position.Y;
            }

            var parameters = new SceneObjectsAddedEventParameters(sharedSceneObjects, GetCharacterInformations(sceneObjects));
            eventSender.Send((byte)GameEvents.SceneObjectsAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscribersRemoved(int[] sceneObjectsId)
        {
            if (!peerGetter.GetPeer().IsConnected)
            {
                return;
            }

            var parameters = new SceneObjectsRemovedEventParameters(sceneObjectsId);
            eventSender.Send((byte)GameEvents.SceneObjectsRemoved, parameters, MessageSendOptions.DefaultReliable());
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
                var peerId = subscriber.Container.GetComponent<IPeerIdGetter>();
                if (peerId == null)
                {
                    continue;
                }

                var peerWrapper = peerContainer.GetPeerWrapper(peerId.GetId());
                if (peerWrapper == null)
                {
                    continue;
                }

                var eventSender = peerWrapper.PeerLogic.Entity.Container.GetComponent<IEventSenderWrapper>().AssertNotNull();
                eventSender.Send(code, parameters, messageSendOptions);
            }
        }

        private IEnumerable<ISceneObject> GetSubscribersFromPublishers
        {
            get
            {
                var subscribers = new List<ISceneObject>();
                var sceneObject = sceneObjectGetter.GetSceneObject();

                var interestArea = sceneObject.Container.GetComponent<IInterestArea>().AssertNotNull();
                if (interestArea == null)
                {
                    return subscribers.ToArray();
                }

                foreach (var publisher in interestArea.GetSubscribedPublishers())
                {
                    subscribers.AddRange(publisher.GetAllSubscribers().Where(subscriber => subscriber.Id != sceneObject.Id));
                }
                return subscribers.ToArray();
            }
        }

        private CharacterInformation? GetCharacterInformation(ISceneObject sceneObject)
        {
            var characterInformationProvider = sceneObject.Container.GetComponent<ICharacterInformationProvider>();
            if (characterInformationProvider == null)
            {
                return null;
            }
            return new CharacterInformation(sceneObject.Id, characterInformationProvider.GetCharacterName(), characterInformationProvider.GetCharacterClass());
        }

        private CharacterInformation[] GetCharacterInformations(IEnumerable<ISceneObject> sceneObjects)
        {
            var characterInformations = new List<CharacterInformation>();

            foreach (var sceneObject in sceneObjects)
            {
                var characterInformationProvider = sceneObject.Container.GetComponent<ICharacterInformationProvider>();
                if (characterInformationProvider == null)
                {
                    continue;
                }

                characterInformations.Add(new CharacterInformation(sceneObject.Id, characterInformationProvider.GetCharacterName(), characterInformationProvider.GetCharacterClass()));
            }
            return characterInformations.ToArray();
        }
    }
}