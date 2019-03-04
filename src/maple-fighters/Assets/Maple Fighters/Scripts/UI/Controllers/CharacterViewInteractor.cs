using System.Threading.Tasks;
using CommonTools.Coroutines;
using Game.Common;
using Scripts.Containers;
using Scripts.Network.APIs;
using Scripts.UI.Models;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    [RequireComponent(typeof(IOnCharacterReceivedListener))]
    [RequireComponent(typeof(IOnCharacterValidatedListener))]
    [RequireComponent(typeof(IOnCharacterRemovedListener))]
    [RequireComponent(typeof(IOnCharacterCreationFinishedListener))]
    public class CharacterViewInteractor : MonoBehaviour
    {
        private ICharacterSelectorApi characterSelectorApi;
        private IOnCharacterReceivedListener onCharacterReceivedListener;
        private IOnCharacterValidatedListener onCharacterValidatedListener;
        private IOnCharacterRemovedListener onCharacterRemovedListener;
        private IOnCharacterCreationFinishedListener onCharacterCreationFinishedListener;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            characterSelectorApi =
                ServiceContainer.GameService.GetCharacterSelectorApi();

            onCharacterReceivedListener =
                GetComponent<IOnCharacterReceivedListener>();
            onCharacterValidatedListener =
                GetComponent<IOnCharacterValidatedListener>();
            onCharacterRemovedListener =
                GetComponent<IOnCharacterRemovedListener>();
            onCharacterCreationFinishedListener =
                GetComponent<IOnCharacterCreationFinishedListener>();

            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.Dispose();
        }

        public void GetCharacters()
        {
            coroutinesExecutor?.StartTask(GetCharactersAsync);
        }

        private async Task GetCharactersAsync(IYield yield)
        {
            onCharacterReceivedListener.OnBeforeCharacterReceived();

            var parameters =
                await characterSelectorApi.GetCharactersAsync(yield);

            var characters = parameters.Characters;
            foreach (var character in characters)
            {
                var characterDetails = new CharacterDetails(
                    character.Name,
                    character.Index.ToUiCharacterIndex(),
                    character.CharacterType.ToUiCharacterClass(),
                    character.LastMap.ToUiMapIndex(),
                    character.HasCharacter);

                onCharacterReceivedListener.OnAfterCharacterReceived(
                    characterDetails);
            }
        }

        public void ValidateCharacter(int characterIndex)
        {
            var parameters =
                new ValidateCharacterRequestParameters(characterIndex);

            coroutinesExecutor.StartTask(
                (yield) => ValidateCharacterAsync(yield, parameters));
        }

        private async Task ValidateCharacterAsync(
            IYield yield,
            ValidateCharacterRequestParameters parameters)
        {
            var responseParameters =
                await characterSelectorApi.ValidateCharacterAsync(
                    yield,
                    parameters);

            var map = responseParameters.Map;
            var status = responseParameters.Status;

            switch (status)
            {
                case CharacterValidationStatus.Ok:
                {
                    onCharacterValidatedListener.OnCharacterValidated(
                        map.ToUiMapIndex());
                    break;
                }

                case CharacterValidationStatus.Wrong:
                {
                    // TODO: Implement
                    break;
                }
            }
        }

        public void RemoveCharacter(int characterIndex)
        {
            var parameters =
                new RemoveCharacterRequestParameters(characterIndex);

            coroutinesExecutor?.StartTask(
                (yield) => RemoveCharacterAsync(yield, parameters));
        }

        private async Task RemoveCharacterAsync(
            IYield yield,
            RemoveCharacterRequestParameters parameters)
        {
            var responseParameters =
                await characterSelectorApi.RemoveCharacterAsync(
                    yield,
                    parameters);

            var status = responseParameters.Status;
            switch (status)
            {
                case RemoveCharacterStatus.Succeed:
                {
                    onCharacterRemovedListener.OnCharacterRemoved();
                    break;
                }

                case RemoveCharacterStatus.Failed:
                {
                    // TODO: Implement
                    break;
                }
            }
        }

        public void CreateCharacter(CharacterDetails characterDetails)
        {
            var parameters = new CreateCharacterRequestParameters(
                characterDetails.GetCharacterClass().FromUiCharacterClass(),
                characterDetails.GetCharacterName(),
                characterDetails.GetCharacterIndex().FromUiCharacterIndex());

            coroutinesExecutor.StartTask(
                (yield) => CreateCharacterAsync(yield, parameters));
        }

        private async Task CreateCharacterAsync(
            IYield yield,
            CreateCharacterRequestParameters parameters)
        {
            var responseParameters =
                await characterSelectorApi.CreateCharacterAsync(
                    yield,
                    parameters);

            var status = responseParameters.Status;
            switch (status)
            {
                case CharacterCreationStatus.Succeed:
                {
                    onCharacterCreationFinishedListener.OnCharacterCreated();
                    break;
                }

                case CharacterCreationStatus.Failed:
                {
                    onCharacterCreationFinishedListener.OnCreateCharacterFailed(
                        CharacterCreationFailed.Unknown);
                    break;
                }

                case CharacterCreationStatus.NameUsed:
                {
                    onCharacterCreationFinishedListener.OnCreateCharacterFailed(
                        CharacterCreationFailed.NameAlreadyInUse);
                    break;
                }
            }
        }
    }
}