using Proyecto26;
using Scripts.Constants;
using Scripts.Services;
using Scripts.Services.CharacterProviderApi;
using UnityEngine;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(IOnCharacterReceivedListener))]
    [RequireComponent(typeof(IOnCharacterValidationFinishedListener))]
    [RequireComponent(typeof(IOnCharacterCreationFinishedListener))]
    [RequireComponent(typeof(IOnCharacterDeletionFinishedListener))]
    public class CharacterViewInteractor : MonoBehaviour
    {
        private ICharacterProviderApi characterProviderApi;
        private IOnCharacterReceivedListener onCharacterReceivedListener;
        private IOnCharacterValidationFinishedListener onCharacterValidationFinishedListener;
        private IOnCharacterCreationFinishedListener onCharacterCreationFinishedListener;
        private IOnCharacterDeletionFinishedListener onCharacterDeletionFinishedListener;

        private void Awake()
        {
            characterProviderApi =
                ApiProvider.ProvideCharacterProviderApi();
            onCharacterReceivedListener =
                GetComponent<IOnCharacterReceivedListener>();
            onCharacterValidationFinishedListener =
                GetComponent<IOnCharacterValidationFinishedListener>();
            onCharacterCreationFinishedListener =
                GetComponent<IOnCharacterCreationFinishedListener>();
            onCharacterDeletionFinishedListener =
                GetComponent<IOnCharacterDeletionFinishedListener>();

            if (characterProviderApi != null)
            {
                characterProviderApi.CreateCharacterCallback += OnCreateCharacterCallback;
                characterProviderApi.DeleteCharacterCallback += OnDeleteCharacterCallback;
                characterProviderApi.GetCharactersCallback += OnGetCharactersCallback;
            }
        }

        private void OnDestroy()
        {
            if (characterProviderApi != null)
            {
                characterProviderApi.CreateCharacterCallback -= OnCreateCharacterCallback;
                characterProviderApi.DeleteCharacterCallback -= OnDeleteCharacterCallback;
                characterProviderApi.GetCharactersCallback -= OnGetCharactersCallback;
            }
        }

        public void CreateCharacter(int index, UINewCharacterDetails characterDetails)
        {
            var userId = UserData.Id;
            var characterIndex = index;
            var characterName = characterDetails.GetCharacterName();
            var classIndex = (int)characterDetails.GetCharacterClass();

            characterProviderApi?.CreateCharacter(userId, characterName, characterIndex, classIndex);
        }

        private void OnCreateCharacterCallback(long statusCode, string json)
        {
            switch (statusCode)
            {
                case 201: // Created
                {
                    onCharacterCreationFinishedListener.OnCharacterCreated();
                    break;
                }

                case 400: // Bad Request
                {
                    var reason = UICharacterCreationFailed.NameAlreadyInUse;

                    onCharacterCreationFinishedListener.OnCreateCharacterFailed(reason);
                    break;
                }

                default:
                {
                    var reason = UICharacterCreationFailed.Unknown;

                    onCharacterCreationFinishedListener.OnCreateCharacterFailed(reason);
                    break;
                }
            }
        }

        public void RemoveCharacter(int characterId)
        {
            characterProviderApi?.DeleteCharacter(characterId);
        }

        private void OnDeleteCharacterCallback(long statusCode, string json)
        {
            onCharacterDeletionFinishedListener.OnCharacterDeletionSucceed();
        }

        public void GetCharacters()
        {
            var userId = UserData.Id;

            characterProviderApi?.GetCharacters(userId);
        }

        private void OnGetCharactersCallback(long statusCode, string json)
        {
            var characters = GetSampleCharacterData();

            // Hack: "[]"
            if (json != "[]")
            {
                var characterData = JsonHelper.FromJsonString<CharacterData>(json);
                if (characterData != null && characterData.Length != 0)
                {
                    // Replaces sample character with existing characters
                    foreach (var character in characterData)
                    {
                        var index = character.index;

                        characters[index].id = character.id;
                        characters[index].charactername = character.charactername;
                        characters[index].classindex = character.classindex;
                    }
                }
            }

            // Create samples and/or existing characters
            foreach (var character in characters)
            {
                var id = character.id;
                var name = character.charactername;
                var index = (UICharacterIndex)character.index;
                var classindex = (UICharacterClass)character.classindex;
                var uiCharacterDetails =
                    new UICharacterDetails(id, name, index, classindex);

                onCharacterReceivedListener.OnCharacterReceived(uiCharacterDetails);
            }

            onCharacterReceivedListener.OnAfterCharacterReceived();
        }

        private CharacterData[] GetSampleCharacterData()
        {
            // NOTE: Make sure the "index" parameter is like this array
            return new CharacterData[3]
            {
                new CharacterData { userid = UserData.Id, charactername = "Sample", index = 0, classindex = 0 },
                new CharacterData { userid = UserData.Id, charactername = "Sample", index = 1, classindex = 0 },
                new CharacterData { userid = UserData.Id, charactername = "Sample", index = 2, classindex = 0 }
            };
        }

        public void ValidateCharacter(byte characterType, string characterName)
        {
            // TODO: Get map name from the server
            var mapName = SceneNames.Maps.Lobby;

            UserData.CharacterData.Type = characterType;
            UserData.CharacterData.Name = characterName;

            onCharacterValidationFinishedListener.OnCharacterValidated(mapName);
        }
    }
}