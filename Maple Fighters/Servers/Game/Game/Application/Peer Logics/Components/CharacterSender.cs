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
    internal class CharacterSender : Component
    {
        private IEventSenderWrapper eventSender;

        protected override void OnAwake()
        {
            base.OnAwake();

            eventSender = Components.GetComponent<IEventSenderWrapper>().AssertNotNull();

            var sceneObjectGetter = Components.GetComponent<ISceneObjectGetter>().AssertNotNull();
            var interestArea = sceneObjectGetter.GetSceneObject().Components.GetComponent<IInterestArea>().AssertNotNull();
            interestArea.SubscriberAdded += OnCharacterAdded;
            interestArea.SubscribersAdded += OnCharactersAdded;
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
            var characterGetter = sceneObject.Components.GetComponent<ICharacterGetter>();
            if (characterGetter == null)
            {
                return null;
            }

            var transform = sceneObject.Components.GetComponent<ITransform>().AssertNotNull();
            return new CharacterSpawnDetailsParameters(sceneObject.Id, characterGetter.GetCharacter(), transform.Direction.GetDirectionsFromDirection());
        }
    }
}