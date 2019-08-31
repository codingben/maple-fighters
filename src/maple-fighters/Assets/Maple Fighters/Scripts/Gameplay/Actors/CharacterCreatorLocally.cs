using Scripts.UI.Controllers;
using Scripts.Utils.Shared;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(CharacterGameObject), typeof(PositionSender))]
    public class PositionSenderInitializer : MonoBehaviour
    {
        private CharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<CharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated(ICharacterGameObjectProvider characterGameObjectProvider)
        {
            var character = characterGameObjectProvider.GetCharacterGameObject();
            var positionSender = GetComponent<PositionSender>();
            positionSender.SetCharacter(character.transform);
        }
    }

    [RequireComponent(typeof(CharacterGameObject))]
    public class PlayerControllerInitializer : MonoBehaviour
    {
        private CharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<CharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated(ICharacterGameObjectProvider characterGameObjectProvider)
        {
            var playerStateAnimator = characterGameObjectProvider
                .GetCharacterSpriteGameObject()
                .AddComponent<PlayerStateAnimator>();
            if (playerStateAnimator != null)
            {
                var playerController = characterGameObjectProvider
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

    [RequireComponent(typeof(CharacterGameObject), typeof(CharacterDetails))]
    public class ChatControllerInitializer : MonoBehaviour
    {
        private CharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<CharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated(ICharacterGameObjectProvider characterGameObjectProvider)
        {
            var chatController = FindObjectOfType<ChatController>();
            if (chatController != null)
            {
                var characterDetailsProvider = GetComponent<ICharacterDetailsProvider>();
                var characterDetails = characterDetailsProvider.GetCharacterDetails();
                var characterName = characterDetails.Character.Name;

                chatController.SetCharacterName(characterName);
            }
        }
    }
}