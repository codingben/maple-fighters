using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using PeerLogic.Common.Components;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Components
{
    internal class CharactersSender : Component
    {
        private IEventSenderWrapper eventSender;

        protected override void OnAwake()
        {
            base.OnAwake();

            eventSender = Entity.GetComponent<IEventSenderWrapper>().AssertNotNull();

            var sceneObjectGetter = Entity.GetComponent<ISceneObjectGetter>().AssertNotNull();
            var interestArea = sceneObjectGetter.GetSceneObject().Container.GetComponent<IInterestArea>().AssertNotNull();
            interestArea.SubscriberAdded += OnCharacterAdded;
            interestArea.SubscribersAdded += OnCharactersAdded;
        }

        private void OnCharacterAdded(ISceneObject character)
        {
            var characterInformation = GetCharacterInformation(character);
            if (characterInformation.HasValue)
            {
                eventSender.Send((byte)GameEvents.CharacterAdded, new CharacterAddedEventParameters(characterInformation.Value), MessageSendOptions.DefaultReliable());
            }
        }

        private void OnCharactersAdded(ISceneObject[] characters)
        {
            var charactersInformation = GetCharactersInformation(characters).ToArray();
            if (charactersInformation.Any())
            {
                eventSender.Send((byte)GameEvents.CharactersAdded, new CharactersAddedEventParameters(charactersInformation), MessageSendOptions.DefaultReliable());
            }
        }

        private CharacterInformation? GetCharacterInformation(ISceneObject sceneObject)
        {
            var characterInformationProvider = sceneObject.Container.GetComponent<ICharacterInformationProvider>();
            if (characterInformationProvider == null)
            {
                return null;
            }

            var orientationProvider = sceneObject.Container.GetComponent<IOrientationProvider>().AssertNotNull();
            return new CharacterInformation(sceneObject.Id, characterInformationProvider.GetCharacterName(), characterInformationProvider.GetCharacterClass(),
                orientationProvider.Direction.GetDirectionsFromDirection());
        }

        private IEnumerable<CharacterInformation> GetCharactersInformation(IEnumerable<ISceneObject> sceneObjects)
        {
            var charactersInformation = new List<CharacterInformation>();

            foreach (var sceneObject in sceneObjects)
            {
                var characterInformationProvider = sceneObject.Container.GetComponent<ICharacterInformationProvider>();
                if (characterInformationProvider == null)
                {
                    continue;
                }

                var orientationProvider = sceneObject.Container.GetComponent<IOrientationProvider>().AssertNotNull();
                charactersInformation.Add(new CharacterInformation(sceneObject.Id, characterInformationProvider.GetCharacterName(), characterInformationProvider.GetCharacterClass(),
                    orientationProvider.Direction.GetDirectionsFromDirection()));
            }

            return charactersInformation.ToArray();
        }
    }
}