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
            // TODO: Implement
        }

        public void GetCharacters()
        {
            // TODO: Implement
        }

        public void ValidateCharacter(int characterIndex)
        {
            // TODO: Implement
        }

        public void RemoveCharacter(int characterIndex)
        {
            // TODO: Implement
        }

        public void CreateCharacter(UICharacterDetails characterDetails)
        {
            // TODO: Implement
        }
    }
}