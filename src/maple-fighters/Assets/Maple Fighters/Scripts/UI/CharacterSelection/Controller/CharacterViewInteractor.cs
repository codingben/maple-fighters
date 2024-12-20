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
            onCharacterReceivedListener =
                GetComponent<IOnCharacterReceivedListener>();
            onCharacterCreationFinishedListener =
                GetComponent<IOnCharacterCreationFinishedListener>();
            onCharacterDeletionFinishedListener =
                GetComponent<IOnCharacterDeletionFinishedListener>();

            SetCharacterProviderApi();
        }

        public void SetCharacterProviderApi()
        {
            UnsubscribeToCharacterProviderApi();

            characterProviderApi = ApiProvider.ProvideCharacterProviderApi();

            SubscribeToCharacterProviderApi();
        }

        private void OnDestroy()
        {
            UnsubscribeToCharacterProviderApi();
        }

        private void SubscribeToCharacterProviderApi()
        {
            if (characterProviderApi != null)
            {
                characterProviderApi.CreateCharacterCallback += OnCreateCharacterCallback;
                characterProviderApi.DeleteCharacterCallback += OnDeleteCharacterCallback;
                characterProviderApi.GetCharactersCallback += OnGetCharactersCallback;
            }
        }

        private void UnsubscribeToCharacterProviderApi()
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
                var level = character.characterlevel;
                var experience = character.characterexperience;
                var index = (UICharacterIndex)character.index;
                var classindex = (UICharacterClass)character.classindex;
                var uiCharacterDetails = new UICharacterDetails(id, name, level, experience, index, classindex);

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

            var characterDataCollection = JsonUtility.FromJson<CharacterDataCollection>(json);
            var characterItems = characterDataCollection.items;

            foreach (var character in characterItems)
            {
                var index = character.index;

                characters[index].id = character.id;
                characters[index].charactername = character.charactername;
                characters[index].characterlevel = character.characterlevel;
                characters[index].characterexperience = character.characterexperience;
                characters[index].classindex = character.classindex;
            }
        }

        private CharacterData[] GetSampleCharacterData()
        {
            // NOTE: Make sure the "index" parameter is like this array
            return new CharacterData[1]
            {
                new CharacterData
                {
                    userid = string.Empty,
                    charactername = "Sample",
                    characterlevel = 1,
                    characterexperience = 0,
                    index = 0,
                    classindex = 0
                }
            };
        }

        public void UpdateCharacterData(int characterId, byte characterType, string characterName, int characterLevel, float characterExperience)
        {
            var userMetadata = FindObjectOfType<UserMetadata>();
            if (userMetadata != null)
            {
                userMetadata.CharacterId = characterId;
                userMetadata.CharacterType = characterType;
                userMetadata.CharacterName = characterName;
                userMetadata.CharacterLevel = characterLevel;
                userMetadata.CharacterExperiencePoints = characterExperience;
            }
        }
    }
}