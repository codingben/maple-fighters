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
            var characterSpawnDetails = GetCharacterSpawnDetails(character);
            if (characterSpawnDetails.HasValue)
            {
                eventSender.Send((byte)GameEvents.CharacterAdded, new CharacterAddedEventParameters(characterSpawnDetails.Value), MessageSendOptions.DefaultReliable());
            }
        }

        private void OnCharactersAdded(ISceneObject[] characters)
        {
            var characterSpawnDetails = GetCharactersSpawnDetails(characters).ToArray();
            if (characterSpawnDetails.Any())
            {
                eventSender.Send((byte)GameEvents.CharactersAdded, new CharactersAddedEventParameters(characterSpawnDetails), MessageSendOptions.DefaultReliable());
            }
        }

        private CharacterSpawnDetails? GetCharacterSpawnDetails(ISceneObject sceneObject)
        {
            // It may be null because not every object on a scene is a character.
            var characterGetter = sceneObject.Container.GetComponent<ICharacterGetter>();
            if (characterGetter == null)
            {
                return null;
            }

            var transform = sceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            return new CharacterSpawnDetails(sceneObject.Id, characterGetter.GetCharacter(), transform.Direction.GetDirectionsFromDirection());
        }

        private IEnumerable<CharacterSpawnDetails> GetCharactersSpawnDetails(IEnumerable<ISceneObject> sceneObjects)
        {
            var charactersSpawnDetails = new List<CharacterSpawnDetails>();

            foreach (var sceneObject in sceneObjects)
            {
                // It may be null because not every object on a scene is a character.
                var characterGetter = sceneObject.Container.GetComponent<ICharacterGetter>();
                if (characterGetter == null)
                {
                    return null;
                }

                var transform = sceneObject.Container.GetComponent<ITransform>().AssertNotNull();
                charactersSpawnDetails.Add(new CharacterSpawnDetails(sceneObject.Id, characterGetter.GetCharacter(), transform.Direction.GetDirectionsFromDirection()));
            }

            return charactersSpawnDetails.ToArray();
        }
    }
}