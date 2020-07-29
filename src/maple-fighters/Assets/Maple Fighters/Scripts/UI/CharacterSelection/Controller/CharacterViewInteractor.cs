using UnityEngine;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(IOnConnectionFinishedListener))]
    [RequireComponent(typeof(IOnCharacterReceivedListener))]
    [RequireComponent(typeof(IOnCharacterValidationFinishedListener))]
    [RequireComponent(typeof(IOnCharacterCreationFinishedListener))]
    [RequireComponent(typeof(IOnCharacterDeletionFinishedListener))]
    public class CharacterViewInteractor : MonoBehaviour
    {
        private IOnConnectionFinishedListener onConnectionFinishedListener;
        private IOnCharacterReceivedListener onCharacterReceivedListener;
        private IOnCharacterValidationFinishedListener onCharacterValidationFinishedListener;
        private IOnCharacterCreationFinishedListener onCharacterCreationFinishedListener;
        private IOnCharacterDeletionFinishedListener onCharacterDeletionFinishedListener;

        private void Awake()
        {
            onConnectionFinishedListener =
                GetComponent<IOnConnectionFinishedListener>();
            onCharacterReceivedListener =
                GetComponent<IOnCharacterReceivedListener>();
            onCharacterValidationFinishedListener =
                GetComponent<IOnCharacterValidationFinishedListener>();
            onCharacterCreationFinishedListener =
                GetComponent<IOnCharacterCreationFinishedListener>();
            onCharacterDeletionFinishedListener =
                GetComponent<IOnCharacterDeletionFinishedListener>();
        }

        public void ConnectToGameServer()
        {
            // TODO: Connect
            // onConnectionFinishedListener.OnConnectionSucceed()
            // onConnectionFinishedListener.OnConnectionFailed()
        }

        public void GetCharacters()
        {
            /*var parameters =
                await characterSelectorApi.GetCharactersAsync(yield);
            var characters = parameters.Characters;

            foreach (var character in characters)
            {
                var characterDetails = new CharacterDetails(
                    character.Name,
                    character.Index.ToUiCharacterIndex(),
                    character.CharacterType.ToUiCharacterClass(),
                    character.LastMap.ToString(),
                    character.HasCharacter);

                onCharacterReceivedListener.OnCharacterReceived(
                    characterDetails);
            }

            onCharacterReceivedListener.OnAfterCharacterReceived();*/
        }

        public void ValidateCharacter(int characterIndex)
        {
            /*var parameters =
                new ValidateCharacterRequestParameters(characterIndex);

            var responseParameters =
                await characterSelectorApi.ValidateCharacterAsync(
                    yield,
                    parameters);
            var status = responseParameters.Status;
            var map = responseParameters.Map;
            var mapName = map.ToString();

            switch (status)
            {
                case CharacterValidationStatus.Ok:
                    {
                        onCharacterValidationFinishedListener
                            .OnCharacterValidated(mapName);
                        break;
                    }

                case CharacterValidationStatus.Wrong:
                    {
                        onCharacterValidationFinishedListener
                            .OnCharacterUnvalidated();
                        break;
                    }
            }*/
        }

        public void RemoveCharacter(int characterIndex)
        {
            /*var parameters =
                new RemoveCharacterRequestParameters(characterIndex);

            var responseParameters =
                await characterSelectorApi.RemoveCharacterAsync(
                    yield,
                    parameters);
            var status = responseParameters.Status;

            switch (status)
            {
                case RemoveCharacterStatus.Succeed:
                    {
                        onCharacterDeletionFinishedListener
                            .OnCharacterDeletionSucceed();
                        break;
                    }

                case RemoveCharacterStatus.Failed:
                    {
                        onCharacterDeletionFinishedListener
                            .OnCharacterDeletionFailed();
                        break;
                    }
            }*/
        }

        public void CreateCharacter(CharacterDetails characterDetails)
        {
            /*var parameters = new CreateCharacterRequestParameters(
                characterDetails.GetCharacterClass().FromUiCharacterClass(),
                characterDetails.GetCharacterName(),
                characterDetails.GetCharacterIndex().FromUiCharacterIndex());

            var responseParameters =
                await characterSelectorApi.CreateCharacterAsync(
                    yield,
                    parameters);
            var status = responseParameters.Status;

            switch (status)
            {
                case CharacterCreationStatus.Succeed:
                    {
                        onCharacterCreationFinishedListener
                            .OnCharacterCreated();
                        break;
                    }

                case CharacterCreationStatus.Failed:
                    {
                        onCharacterCreationFinishedListener
                            .OnCreateCharacterFailed(CharacterCreationFailed.Unknown);
                        break;
                    }

                case CharacterCreationStatus.NameUsed:
                    {
                        onCharacterCreationFinishedListener
                            .OnCreateCharacterFailed(CharacterCreationFailed.NameAlreadyInUse);
                        break;
                    }
            }*/
        }
    }
}