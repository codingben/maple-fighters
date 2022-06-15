using Proyecto26;
using Scripts.Services;
using Scripts.Services.CharacterProviderApi;
using UnityEngine;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(IOnCharacterReceivedListener))]
    [RequireComponent(typeof(IOnCharacterCreationFinishedListener))]
    [RequireComponent(typeof(IOnCharacterDeletionFinishedListener))]
    public class CharacterViewInteractor : MonoBehaviour
    {
        private ICharacterProviderApi characterProviderApi;
        private IOnCharacterReceivedListener onCharacterReceivedListener;
        private IOnCharacterCreationFinishedListener onCharacterCreationFinishedListener;
        private IOnCharacterDeletionFinishedListener onCharacterDeletionFinishedListener;

        private void Awake()
        {
            characterProviderApi =
                ApiProvider.ProvideCharacterProviderApi();
            onCharacterReceivedListener =
                GetComponent<IOnCharacterReceivedListener>();
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
            var userMetadata = FindObjectOfType<UserMetadata>();
            var userId = userMetadata?.UserData.id ?? string.Empty;
            var characterIndex = index;
            var characterName = characterDetails.GetCharacterName();
            var classIndex = (int)characterDetails.GetCharacterClass();

            characterProviderApi?.CreateCharacter(userId, characterName, characterIndex, classIndex);
        }

        private void OnCreateCharacterCallback(long statusCode, string json)
        {
            switch (statusCode)
            {
                case (long)StatusCodes.Created:
                {
                    onCharacterCreationFinishedListener.OnCharacterCreated();
                    break;
                }

                case (long)StatusCodes.BadRequest:
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
            if (statusCode == (long)StatusCodes.Ok)
            {
                onCharacterDeletionFinishedListener.OnCharacterDeletionSucceed();
            }
        }

        public void GetCharacters()
        {
            var userMetadata = FindObjectOfType<UserMetadata>();
            var userId = userMetadata?.UserData.id ?? string.Empty;

            characterProviderApi?.GetCharacters(userId);
        }

        private void OnGetCharactersCallback(long statusCode, string json)
        {
            var characters = GetSampleCharacterData();

            if (statusCode == (long)StatusCodes.Ok)
            {
                SetCharacters(ref characters, json);
            }

            foreach (var character in characters)
            {
                var id = character.id;
                var name = character.charactername;
                var index = (UICharacterIndex)character.index;
                var classindex = (UICharacterClass)character.classindex;
                var uiCharacterDetails = new UICharacterDetails(id, name, index, classindex);

                onCharacterReceivedListener.OnCharacterReceived(uiCharacterDetails);
            }

            onCharacterReceivedListener.OnAfterCharacterReceived();
        }

        private void SetCharacters(ref CharacterData[] characters, string json)
        {
            if (json == "[]" || string.IsNullOrEmpty(json))
            {
                return;
            }

            CharacterData[] characterData;

            // NOTE: Unity json allows getting "Items" only (from dummy api)
            if (json.Contains("Items"))
            {
                characterData = JsonHelper.FromJsonString<CharacterData>(json);
            }
            else
            {
                characterData = JsonHelper.ArrayFromJson<CharacterData>(json);
            }

            if (characterData == null || characterData?.Length == 0)
            {
                return;
            }

            foreach (var character in characterData)
            {
                var index = character.index;

                characters[index].id = character.id;
                characters[index].charactername = character.charactername;
                characters[index].classindex = character.classindex;
            }
        }

        private CharacterData[] GetSampleCharacterData()
        {
            // NOTE: Make sure the "index" parameter is like this array
            return new CharacterData[3]
            {
                new CharacterData { userid = string.Empty, charactername = "Sample", index = 0, classindex = 0 },
                new CharacterData { userid = string.Empty, charactername = "Sample", index = 1, classindex = 0 },
                new CharacterData { userid = string.Empty, charactername = "Sample", index = 2, classindex = 0 }
            };
        }

        public void UpdateCharacterData(byte characterType, string characterName)
        {
            var userMetadata = FindObjectOfType<UserMetadata>();
            if (userMetadata != null)
            {
                userMetadata.CharacterType = characterType;
                userMetadata.CharacterName = characterName;
            }
        }
    }
}