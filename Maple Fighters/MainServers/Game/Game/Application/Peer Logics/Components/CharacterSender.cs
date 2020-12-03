using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.PeerLogic.Components.Interfaces;
using Game.Application.GameObjects.Components.Interfaces;
using Game.Common;
using InterestManagement.Components.Interfaces;
using PeerLogic.Common.Components.Interfaces;

namespace Game.Application.PeerLogic.Components
{
    internal class CharacterSender : Component
    {
        private IEventSenderWrapper eventSender;

        protected override void OnAwake()
        {
            base.OnAwake();

            eventSender = Components.GetComponent<IEventSenderWrapper>().AssertNotNull();

            SubscribeToInterestAreaEvents();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnsubscribeFromInterestAreaEvents();
        }

        private void SubscribeToInterestAreaEvents()
        {
            var playerGameObjectGetter = Components.GetComponent<IPlayerGameObjectGetter>().AssertNotNull();
            var interestArea = playerGameObjectGetter.GetPlayerGameObject().Components.GetComponent<IInterestAreaEvents>().AssertNotNull();
            interestArea.SubscriberAdded += OnCharacterAdded;
            interestArea.SubscribersAdded += OnCharactersAdded;
        }

        private void UnsubscribeFromInterestAreaEvents()
        {
            var playerGameObjectGetter = Components.GetComponent<IPlayerGameObjectGetter>().AssertNotNull();
            var interestArea = playerGameObjectGetter.GetPlayerGameObject().Components.GetComponent<IInterestAreaEvents>().AssertNotNull();
            interestArea.SubscriberAdded -= OnCharacterAdded;
            interestArea.SubscribersAdded -= OnCharactersAdded;
        }

        private void OnCharacterAdded(ISceneObject character)
        {
            var characterSpawnDetails = GetCharacterSpawnDetails(character);
            if (characterSpawnDetails.HasValue)
            {
                eventSender.Send((byte)GameEvents.CharacterAdded, new CharacterAddedEventParameters(characterSpawnDetails.Value), MessageSendOptions.DefaultReliable());
            }
        }

        private void OnCharactersAdded(ISceneObject[] characters)
        {
            var characterSpawnDetails = GetCharactersSpawnDetails(characters)?.ToArray();
            if (characterSpawnDetails != null && characterSpawnDetails.Any())
            {
                eventSender.Send((byte)GameEvents.CharactersAdded, new CharactersAddedEventParameters(characterSpawnDetails), MessageSendOptions.DefaultReliable());
            }
        }

        private CharacterSpawnDetailsParameters? GetCharacterSpawnDetails(ISceneObject sceneObject)
        {
            var parameters = CreateCharacterSpawnDetails(sceneObject);
            return parameters;
        }

        private IEnumerable<CharacterSpawnDetailsParameters> GetCharactersSpawnDetails(IEnumerable<ISceneObject> sceneObjects)
        {
            var charactersSpawnDetails = new List<CharacterSpawnDetailsParameters>();

            foreach (var sceneObject in sceneObjects)
            {
                var parameters = CreateCharacterSpawnDetails(sceneObject);
                if (parameters.HasValue)
                {
                    charactersSpawnDetails.Add(parameters.Value);
                }
            }

            return charactersSpawnDetails.ToArray();
        }

        private CharacterSpawnDetailsParameters? CreateCharacterSpawnDetails(ISceneObject sceneObject)
        {
            // It may be null because not every object on a scene is a character.
            var characterGetter = sceneObject.Components.GetComponent<ICharacterParametersGetter>();
            if (characterGetter == null)
            {
                return null;
            }

            var directionTransform = sceneObject.Components.GetComponent<IDirectionTransform>().AssertNotNull();
            return new CharacterSpawnDetailsParameters(sceneObject.Id, characterGetter.GetCharacter(), directionTransform.Direction.GetDirectionsFromDirection());
        }
    }
}