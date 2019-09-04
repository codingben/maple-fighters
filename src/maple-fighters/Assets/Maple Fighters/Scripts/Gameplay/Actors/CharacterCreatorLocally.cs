using Scripts.UI.Controllers;
using Scripts.Utils.Shared;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(SpawnCharacter), typeof(PositionSender))]
    public class PositionSenderInitializer : MonoBehaviour
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
            var character = spawnedCharacter.GetCharacterGameObject();
            var positionSender = GetComponent<PositionSender>();
            positionSender.SetCharacter(character.transform);
        }
    }

    [RequireComponent(typeof(SpawnCharacter))]
    public class PlayerControllerInitializer : MonoBehaviour
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
            var playerStateAnimator = spawnedCharacter
                .GetCharacterSpriteGameObject()
                .AddComponent<PlayerStateAnimator>();
            if (playerStateAnimator != null)
            {
                var playerController = spawnedCharacter
                    .GetCharacterGameObject().GetComponent<PlayerController>();
                if (playerController != null)
                {
                    // TODO: Wtf? Who will unsubscribe from there?
                    playerController.PlayerStateChanged +=
                        playerStateAnimator.OnPlayerStateChanged;
                }
            }
        }
    }

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