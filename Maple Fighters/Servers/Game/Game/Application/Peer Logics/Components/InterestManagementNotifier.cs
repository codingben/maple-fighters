using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using PeerLogic.Common.Components;
using Shared.Game.Common;
using SceneObject = Shared.Game.Common.SceneObject;

namespace Game.Application.PeerLogic.Components
{
    internal class InterestManagementNotifier : Component
    {
        private IEventSenderWrapper eventSender;
        private ICharacterGetter sceneObjectGetter;

        protected override void OnAwake()
        {
            base.OnAwake();

            eventSender = Entity.GetComponent<IEventSenderWrapper>().AssertNotNull();
            sceneObjectGetter = Entity.GetComponent<ICharacterGetter>().AssertNotNull();

            var interestArea = sceneObjectGetter.GetSceneObject().Container.GetComponent<IInterestArea>().AssertNotNull();
            interestArea.SubscriberAdded += OnSubscriberAdded;
            interestArea.SubscriberRemoved += OnSubscriberRemoved;
            interestArea.SubscribersAdded += OnSubscribersAdded;
            interestArea.SubscribersRemoved += OnSubscribersRemoved;
        }

        private void OnSubscriberAdded(ISceneObject sceneObject)
        {
            var transform = sceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            var orientationProvider = sceneObject.Container.GetComponent<IOrientationProvider>().AssertNotNull();

            var direction = orientationProvider.Direction.GetDirectionsFromDirection();
            var sharedSceneObject = new SceneObject(sceneObject.Id, sceneObject.Name, transform.Position.X, transform.Position.Y, direction);

            var characterInformation = GetCharacterInformation(sceneObject);
            var parameters = new SceneObjectAddedEventParameters(sharedSceneObject, characterInformation.GetValueOrDefault(), characterInformation.HasValue);
            eventSender.Send((byte)GameEvents.SceneObjectAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscriberRemoved(int subscriberId)
        {
            var parameters = new SceneObjectRemovedEventParameters(subscriberId);
            eventSender.Send((byte)GameEvents.SceneObjectRemoved, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscribersAdded(IReadOnlyList<ISceneObject> sceneObjects)
        {
            var sharedSceneObjects = new SceneObject[sceneObjects.Count];
            for (var i = 0; i < sceneObjects.Count; i++)
            {
                sharedSceneObjects[i].Id = sceneObjects[i].Id;
                sharedSceneObjects[i].Name = sceneObjects[i].Name;

                var transform = sceneObjects[i].Container.GetComponent<ITransform>().AssertNotNull();
                var orientationProvider = sceneObjects[i].Container.GetComponent<IOrientationProvider>().AssertNotNull();

                sharedSceneObjects[i].X = transform.Position.X;
                sharedSceneObjects[i].Y = transform.Position.Y;
                sharedSceneObjects[i].Direction = orientationProvider.Direction.GetDirectionsFromDirection();
            }

            var parameters = new SceneObjectsAddedEventParameters(sharedSceneObjects, GetCharacterInformations(sceneObjects));
            eventSender.Send((byte)GameEvents.SceneObjectsAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscribersRemoved(int[] sceneObjectsId)
        {
            var parameters = new SceneObjectsRemovedEventParameters(sceneObjectsId);
            eventSender.Send((byte)GameEvents.SceneObjectsRemoved, parameters, MessageSendOptions.DefaultReliable());
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