using Scripts.Services.CharacterProvider;
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
        private ICharacterProviderApi characterProviderApi;
        private IOnConnectionFinishedListener onConnectionFinishedListener;
        private IOnCharacterReceivedListener onCharacterReceivedListener;
        private IOnCharacterValidationFinishedListener onCharacterValidationFinishedListener;
        private IOnCharacterCreationFinishedListener onCharacterCreationFinishedListener;
        private IOnCharacterDeletionFinishedListener onCharacterDeletionFinishedListener;

        // TODO: Remove
        private UICharacterDetails[] characters;

        private void Awake()
        {
            characterProviderApi =
                FindObjectOfType<CharacterProviderApi>();
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

        private void Start()
        {
            // TODO: Get this data from server
            characters = new UICharacterDetails[]
            {
                new UICharacterDetails("Knight", UICharacterIndex.First, UICharacterClass.Knight, "Map_1", false),
                new UICharacterDetails("Arrow", UICharacterIndex.Second, UICharacterClass.Arrow, "Map_1", false),
                new UICharacterDetails("Wizard", UICharacterIndex.Third, UICharacterClass.Wizard, "Map_1", false)
            };
        }

        public void ConnectToGameServer()
        {
            // TODO: Get this data from server
            onConnectionFinishedListener.OnConnectionSucceed();
        }

        public void GetCharacters()
        {
            // TODO: Get this data from server
            foreach (var character in characters)
            {
                onCharacterReceivedListener.OnCharacterReceived(character);
            }

            onCharacterReceivedListener.OnAfterCharacterReceived();

        }

        public void ValidateCharacter(int characterIndex)
        {
            // TODO: Get this data from server
            onCharacterValidationFinishedListener.OnCharacterValidated(mapName: "Map_1");
        }

        public void RemoveCharacter(int characterIndex)
        {
            characters[characterIndex].SetHasCharacter(false);

            // TODO: Get this data from server
            onCharacterDeletionFinishedListener.OnCharacterDeletionSucceed();
        }

        public void CreateCharacter(UICharacterDetails characterDetails)
        {
            characterDetails.SetHasCharacter(true);

            var index = (int)characterDetails.GetCharacterIndex();
            characters[index] = characterDetails;

            // TODO: Get this data from server
            onCharacterCreationFinishedListener.OnCharacterCreated();
        }
    }
}