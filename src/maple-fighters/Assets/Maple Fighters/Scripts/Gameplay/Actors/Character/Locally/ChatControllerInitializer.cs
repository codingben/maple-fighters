using Scripts.UI.Controllers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(SpawnCharacter), typeof(SpawnedCharacterDetails))]
    public class ChatControllerInitializer : MonoBehaviour
    {
        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
        }

        private void Start()
        {
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnDestroy()
        {
            spawnedCharacter.CharacterSpawned -= OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var chatController = FindObjectOfType<ChatController>();
            if (chatController != null)
            {
                var characterDetailsProvider = GetComponent<ISpawnedCharacterDetails>();
                var characterDetails = characterDetailsProvider.GetCharacterDetails();
                var characterName = characterDetails.Character.Name;

                chatController.SetCharacterName(characterName);
            }
        }
    }
}