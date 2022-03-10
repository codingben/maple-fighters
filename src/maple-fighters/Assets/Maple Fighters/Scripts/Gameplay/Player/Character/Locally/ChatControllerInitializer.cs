using Scripts.UI.Chat;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter), typeof(CharacterDataProvider))]
    public class ChatControllerInitializer : MonoBehaviour
    {
        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var chatController = FindObjectOfType<ChatController>();
            if (chatController != null)
            {
                var characterDataProvider = GetComponent<ICharacterDataProvider>();
                var characterData = characterDataProvider.GetCharacterData();
                var name = characterData.Name;

                chatController.SetCharacterName(name);
            }
        }
    }
}